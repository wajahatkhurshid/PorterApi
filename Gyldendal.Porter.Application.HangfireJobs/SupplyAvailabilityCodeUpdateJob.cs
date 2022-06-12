using System.Threading.Tasks;
using Gyldendal.Porter.Application.Services.SupplyAvailabilityCode;
using Hangfire;
using Hangfire.Server;
using MediatR;

namespace Gyldendal.Porter.Application.HangfireJobs
{
    public class SupplyAvailabilityCodeUpdateJob
    {
        private readonly IMediator _mediator;

        public SupplyAvailabilityCodeUpdateJob(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Public Methods

        /// <summary>
        /// Kicks off updates for SupplyAvailabilityCode taxonomy 
        /// </summary>
        [DisableConcurrentExecution(int.MaxValue)]
        [AutomaticRetry(Attempts = 3, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public async Task Execute(PerformContext context)
        {
            await _mediator.Send(new SupplyAvailabilityCodeUpdateCommand());
        }

        #endregion
    }
}
