using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Gyldendal.Porter.Common.Configurations;
using Gyldendal.Porter.Domain.Contracts.Entities.Taxonomy;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using Gyldendal.Porter.Infrastructure.ExternalClients.Interfaces;
using Newtonsoft.Json;

namespace Gyldendal.Porter.Infrastructure.ExternalClients.Gpm
{
    /// <summary>
    /// Client for fetching data from the GPM REST API
    /// </summary>
    public class GpmApiClient : IGpmApiClient
    {
        private readonly HttpClient _client;
        private readonly ITaxonomyResponseRepository _responseRepository;

        public GpmApiClient(HttpClient client, GpmConfiguration configuration, ITaxonomyResponseRepository responseRepository = null)
        {
            client.BaseAddress ??= configuration.BaseUri;
            client.DefaultRequestHeaders.Add("Authorization", AppConfigurations.Configuration.GpmConfig.GpmApiKey);
            client.DefaultRequestHeaders.Add("username", configuration.Username);
            client.DefaultRequestHeaders.Add("role", configuration.Role);
            _client = client;
            _responseRepository = responseRepository;
        }

        public async Task<TaxonomyDataOutDto> GetTaxonomyAsync(int taxonomyId, IEnumerable<int> fromNodeIds,
            int? numberOfLevels, CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/Taxonomy/{taxonomyId}");
            var response = await _client.SendAsync(request, cancellationToken);
            var body = await response.Content.ReadAsStringAsync();
            if (_responseRepository != null)
                await Task.Run(async () => await LogGpmResponse(taxonomyId, body), cancellationToken);
            var taxonomy = JsonConvert.DeserializeObject<TaxonomyDataOutDto>(body);

            return taxonomy;
        }

        public async Task<bool> TriggerReplayAsync(int subscriptionId)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, $"/api/Subscription/Replay/{subscriptionId}");
            var response = await _client.SendAsync(request);
            return response.IsSuccessStatusCode;
        }

        public async Task<string> FetchBusinessObjectPayloadAsync(string subscriptionScopeId, string businessObjectId)
        {
            var response = await _client.GetAsync(
                $"/api/Subscriber/scope/{subscriptionScopeId}/businessObject/{businessObjectId}");

            if (!response.IsSuccessStatusCode) return null;

            var businessObjectPayload = await response.Content.ReadAsStringAsync();
            return businessObjectPayload;
        }

        /// <summary>
        /// Simply logs the response from the GPM API for debugging purposes
        /// </summary>
        private async Task LogGpmResponse(int taxonomyId, string responseBody)
        {
            var taxonomyResponse = new TaxonomyResponse
            {
                TaxonomyId = taxonomyId, ResponsePayload = responseBody
            };
            await _responseRepository.InsertTaxonomyResponseAsync(taxonomyResponse);
        }
    }
}