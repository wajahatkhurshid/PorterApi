using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Azure.Messaging.ServiceBus.Administration;
using Gyldendal.Porter.Common;
using Gyldendal.Porter.Common.Configurations;
using Gyldendal.Porter.Common.Response;
using Gyldendal.Porter.Common.Subscription;
using Gyldendal.Porter.Domain.Contracts.Entities.Subscription;
using Gyldendal.Porter.Domain.Contracts.Interfaces;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using Newtonsoft.Json;

namespace Gyldendal.Porter.Infrastructure.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private const string JsonPath = "./Jsons/";
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly AppConfigurations _configuration;

        public SubscriptionService(ISubscriptionRepository subscriptionRepository, AppConfigurations configuration)
        {
            _subscriptionRepository = subscriptionRepository;
            _configuration = configuration;
        }

        public async Task<GpmSubscriptionResponse> Create()
        {
            var subscriptionName = SubscriptionHelper.GetSubscriptionName(_configuration.GpmConfig.IsLocalDevelopment, _configuration.GpmConfig.SubscriptionName);
            var gpmBaseUrl = GetGpmBaseUrl();
            var response = new GpmSubscriptionResponse();

            var subscriptionResult = await CreateSubscriber(gpmBaseUrl, subscriptionName);
            response.SubscriptionResult = subscriptionResult;

            var deserializedResultContent =
                System.Text.Json.JsonSerializer.Deserialize<GpmResponse>(subscriptionResult);
            var subscriptionId = deserializedResultContent.id;

            if (subscriptionId <= 0)
            {
                return response;
            }

            var (productScopeResult, workScopeResult, merchandiseScopeResult, profileScopeResult, seriesScopeResult) =
                await CreateSubscriberScopes(subscriptionId, gpmBaseUrl);
            response.ProductScopeResult = productScopeResult;
            response.WorkScopeResult = workScopeResult;
            response.MerchandiseScopeResult = merchandiseScopeResult;
            response.ProfileScopeResult = profileScopeResult;
            response.SeriesScopeResult = seriesScopeResult;

            await CreateAzureServiceBusSubscription(_configuration.GpmConfig.TopicName,
                subscriptionName, subscriptionId, _configuration.GpmConfig.ServiceBusConnectionString);
            await PersistSubscriber(subscriptionName, subscriptionId, gpmBaseUrl, response);

            return response;
        }

        private async Task<Response<SubscriptionProperties>> CreateAzureServiceBusSubscription(string topicName,
            string subscriptionName,
            int correlationFilterSubscriptionId,
            string connectionString)
        {
            var client = new ServiceBusAdministrationClient(connectionString);

            if (await client.SubscriptionExistsAsync(topicName, subscriptionName))
            {
                await client.DeleteSubscriptionAsync(topicName, subscriptionName);
            }

            var filter = new CorrelationRuleFilter();
            filter.ApplicationProperties.Add("subscription-id", correlationFilterSubscriptionId.ToString());
            var ruleOptions = new CreateRuleOptions("subscription-id-correlation-filter", filter);
            var result = await client.CreateSubscriptionAsync(new CreateSubscriptionOptions(topicName, subscriptionName)
            {
                DefaultMessageTimeToLive = TimeSpan.FromDays(200)
            }, ruleOptions);

            return result;
        }

        public async Task<List<Subscription>> GetSubscriptions()
        {
            return await _subscriptionRepository.GetAllAsync();
        }

        private async Task PersistSubscriber(string subscriptionName, int subscriptionId,
            string gpmUrl, GpmSubscriptionResponse response)
        {
            var subscription = new Subscription
            {
                Id = subscriptionId.ToString(),
                GpmUrl = gpmUrl,
                Name = subscriptionName,
                Scopes = new List<Scope>
                {
                    new Scope
                    {
                        Id = Utils.Deserialize<GpmResponse>(response.ProductScopeResult)?.id.ToString(),
                        Name = "Porter Product Scope"
                    },
                    new Scope
                    {
                        Id = Utils.Deserialize<GpmResponse>(response.WorkScopeResult)?.id.ToString(),
                        Name = "Porter Work Scope"
                    },
                    new Scope
                    {
                        Id = Utils.Deserialize<GpmResponse>(response.MerchandiseScopeResult)?.id.ToString(),
                        Name = "Porter Merchandise Scope"
                    },
                    new Scope
                    {
                        Id = Utils.Deserialize<GpmResponse>(response.ProfileScopeResult)?.id.ToString(),
                        Name = "Porter Profile Scope"
                    },
                    new Scope
                    {
                        Id = Utils.Deserialize<GpmResponse>(response.SeriesScopeResult)?.id.ToString(),
                        Name = "Porter Series Scope"
                    }
                }
            };

            await _subscriptionRepository.UpsertAsync(subscription);
        }

        private string GetGpmBaseUrl()
        {
            return _configuration.GpmConfig.GpmUrl;
        }

        private static async Task<string> CreateSubscriber(string gpmBaseUrl, string subscriptionName)
        {
            var subscriptionRequestJson = File.ReadAllText($"{JsonPath}SubscriptionRequest.json");

            dynamic jsonObj = JsonConvert.DeserializeObject(subscriptionRequestJson);
            jsonObj["name"] = subscriptionName;

            var transformedRequest = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);

            var httpRequest = GetHttpRequest($"{gpmBaseUrl}/api/Subscription", transformedRequest);
            return await GetResponse(gpmBaseUrl, httpRequest);
        }

        private static async
            Task<(string productScopeResult, string workScopeResult, string merchandiseScopeResult, string
                profileScopeResult, string seriesScopeResult)> CreateSubscriberScopes(int subscriptionId,
                string baseUri)
        {
            var productScopeResult = await CreateGpmScope($"{JsonPath}ProductScopeRequest.json", subscriptionId,
                baseUri);
            var workScopeResult = await CreateGpmScope($"{JsonPath}WorkScopeRequest.json", subscriptionId, baseUri);
            var merchandiseScopeResult = await CreateGpmScope($"{JsonPath}MerchandiseScopeRequest.json", subscriptionId,
                baseUri);
            var seriesScopeResult = await CreateGpmScope($"{JsonPath}SeriesScopeRequest.json", subscriptionId,
                baseUri);
            var profileScopeResult = await CreateGpmScope($"{JsonPath}ProfileScopeRequest.json", subscriptionId,
                baseUri);

            return (productScopeResult, workScopeResult, merchandiseScopeResult, profileScopeResult, seriesScopeResult);
        }

        private static async Task<string> CreateGpmScope(string jsonFileWithPath, int subscriptionId, string gpmBaseUri)
        {
            var request = GetTransformedRequest(jsonFileWithPath, subscriptionId);
            var httpRequest = GetHttpRequest($"{gpmBaseUri}/api/Subscription/Scope", request);
            return await GetResponse(gpmBaseUri, httpRequest);
        }

        private static string GetTransformedRequest(string jsonFileWithPath, int subscriptionId)
        {
            var scopeRequestJson = File.ReadAllText(jsonFileWithPath);

            dynamic jsonObj = JsonConvert.DeserializeObject(scopeRequestJson);
            jsonObj["parentSubscriptionId"] = subscriptionId;

            return JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
        }

        private static HttpRequestMessage GetHttpRequest(string uri, string request)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, new Uri(uri));
            httpRequest.Headers.Add("username", "swagger");
            httpRequest.Headers.Add("role", "SuperAdmin");
            httpRequest.Content = new StringContent(request, Encoding.UTF8, "application/json");

            return httpRequest;
        }

        private static async Task<string> GetResponse(string uri, HttpRequestMessage request)
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(uri)
            };

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("Authorization", AppConfigurations.Configuration.GpmConfig.GpmApiKey);
            var result = await httpClient.SendAsync(request);
            var resultContent = await result.Content.ReadAsStringAsync();

            return resultContent;
        }
    }
}