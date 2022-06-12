using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Gyldendal.Porter.Application.Services.Interfaces;
using Hangfire.Server;

namespace Gyldendal.Porter.Application.HangfireJobs
{
    public class ServicebusListenerJob
    {
        private readonly IReceiveGpmMessageService _receiveGpmMessageService;

        public ServicebusListenerJob(IReceiveGpmMessageService receiveGpmMessageService)
        {
            _receiveGpmMessageService = receiveGpmMessageService;
        }

        public async Task Execute(PerformContext context)
        {
            var shutdownToken = context.CancellationToken.ShutdownToken;
            await _receiveGpmMessageService.Receive(shutdownToken).ConfigureAwait(false);
        }
    }
}
