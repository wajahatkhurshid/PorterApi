using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using Gyldendal.Porter.Common;
using Gyldendal.Porter.Common.Configurations;
using Hangfire;

namespace Gyldendal.Porter.Application.CookingProcessor.Worker
{
    public class CookingBackgroundServerWorker : BackgroundService
    {
        private readonly ILogger _logger;

        public CookingBackgroundServerWorker(ILogger logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var workerCount = AppConfigurations.Configuration.AppSettings.WorkerCount;
            var useProcessors = AppConfigurations.Configuration.AppSettings.UseAllAvailableProcessors;
            _logger.Info($"UseAllAvailableCores setting is: {useProcessors} {Environment.MachineName}");

            if (useProcessors)
            {
                _logger.Info($"Available cores are: {Environment.ProcessorCount} {Environment.MachineName}");
                workerCount = Environment.ProcessorCount * workerCount;
            }

            _logger.Info($"WorkerCount will be: {workerCount} {Environment.MachineName}");

            using var server = new BackgroundJobServer(new BackgroundJobServerOptions()
            {
                Queues = new [] { "cooking"},
                WorkerCount = workerCount
            });

            _logger.Info($"Hangfire Server started. {Environment.MachineName}");
            //Console.ReadKey();
            await Task.Delay(-1);
        }
    }
}
