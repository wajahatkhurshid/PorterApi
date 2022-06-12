using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Gyldendal.Porter.Application.Services.Interfaces;
using Gyldendal.Porter.Common;
using Gyldendal.Porter.Common.Configurations;
using Gyldendal.Porter.Common.Subscription;
using Gyldendal.Porter.Infrastructure.ExternalClients.Interfaces;
using MediatR;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Gyldendal.Porter.Application.Services.GpmMessageReceiver
{
    public class ReceiveGpmMessageService : IReceiveGpmMessageService
    {
        private readonly ILogger _logger;
        private readonly IGpmApiClient _gpmApiClient;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly GpmConfig _gpmConfig = AppConfigurations.Configuration.GpmConfig;
        private readonly ServiceBusProcessor _sbProcessor;
        private readonly ServiceBusClient _sbClient;

        public ReceiveGpmMessageService(IAzureClientFactory<ServiceBusClient> azureClientFactory, ILogger logger,
            IGpmApiClient gpmApiClient, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _gpmApiClient = gpmApiClient;
            _serviceScopeFactory = serviceScopeFactory;
            _sbClient = azureClientFactory.CreateClient(_gpmConfig.ServiceBusClient);
            var subscriptionName =
                SubscriptionHelper.GetSubscriptionName(_gpmConfig.IsLocalDevelopment, _gpmConfig.SubscriptionName);
            _sbProcessor = _sbClient.CreateProcessor(_gpmConfig.TopicName, subscriptionName,
                new ServiceBusProcessorOptions { MaxConcurrentCalls = 50, PrefetchCount = 50 });
        }

        /// <summary>
        /// Starts up the service bus processor.
        /// The method takes a cancellation token, which will keep the method running until the cancellation token is cancelled.
        /// This enables us to start a long running hangfire job that will shut down then processor upon stopping the job
        /// </summary>
        public async Task Receive(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (!_sbProcessor.IsProcessing)
                {
                    _sbProcessor.ProcessMessageAsync += ProcessMessageAsync;
                    _sbProcessor.ProcessErrorAsync += ProcessErrorAsync;
                    await _sbProcessor.StartProcessingAsync().ConfigureAwait(false);
                }
            }

            // Clean up
            try
            {
                await _sbProcessor.StopProcessingAsync();
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to stop ASB processor", ex);
            }
            finally
            {
                await _sbProcessor.DisposeAsync();
            }
        }

        private async Task ProcessMessageAsync(ProcessMessageEventArgs args)
        {
            // Using the service scope factory is the recommended way of enabling the usage of a scoped service in a delegate method like this.
            // Without this, the injected MediatR will be disposed in the ContainerService for the next request
            using (var sc = _serviceScopeFactory.CreateScope())
            {
                try
                {
                    var values = ReadGpmMessagePropertiesFromAsbMessage(args);
                    if (values.ChangeType.Equals("Update", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var payload =
                            await _gpmApiClient.FetchBusinessObjectPayloadAsync(values.AffectedScopeId,
                                values.BusinessObjectId).ConfigureAwait(false);
                        var containerService = sc.ServiceProvider.GetRequiredService<IContainerService>();
                        await containerService.AddContainer(values.ContainerTypeId, payload).ConfigureAwait(false);
                    }

                    await args.CompleteMessageAsync(args.Message).ConfigureAwait(false);
                    _logger.Info(
                        $"Received and Completed message {values.BusinessObjectId} with message ID {args.Message.MessageId}");
                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to fetch data for message with ID {args.Message.MessageId}", ex);
                }
            }
        }

        private async Task ProcessErrorAsync(ProcessErrorEventArgs args)
        {
            _logger.Error($"{args.Exception.Message}", args.Exception);
        }

        private static GpmMessageProperties ReadGpmMessagePropertiesFromAsbMessage(ProcessMessageEventArgs message)
        {
            var values = message.Message.ApplicationProperties;
            var gpmMessageProperties = new GpmMessageProperties();
            if (values.TryGetValue("businessobject-id", out var businessObjectId))
                gpmMessageProperties.BusinessObjectId = businessObjectId.ToString();
            if (values.TryGetValue("affected-scope", out var scopeId))
                gpmMessageProperties.AffectedScopeId = scopeId.ToString();
            if (values.TryGetValue("businessoject-version", out var businessObjectVersion))
                gpmMessageProperties.BusinessObjectVersion = businessObjectVersion.ToString();
            if (values.TryGetValue("change-type", out var changeType))
                gpmMessageProperties.ChangeType = changeType.ToString();
            if (values.TryGetValue("container-type-id", out var containerTypeId))
                gpmMessageProperties.ContainerTypeId = containerTypeId.ToString();
            if (values.TryGetValue("subscription-id", out var subscriptionId))
                gpmMessageProperties.SubscriptionId = subscriptionId.ToString();
            if (values.TryGetValue("timestamp", out var timestamp))
                gpmMessageProperties.Timestamp = timestamp.ToString();
            return gpmMessageProperties;
        }
    }

    public class GpmMessageProperties
    {
        public string BusinessObjectId { get; set; }
        public string AffectedScopeId { get; set; }
        public string BusinessObjectVersion { get; set; }
        public string ChangeType { get; set; }
        public string ContainerTypeId { get; set; }
        public string SubscriptionId { get; set; }
        public string Timestamp { get; set; }
    }
}