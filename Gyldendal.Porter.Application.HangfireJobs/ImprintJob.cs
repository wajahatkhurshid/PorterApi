using Hangfire;
using MediatR;
using System.Threading.Tasks;
using Hangfire.Server;
using Gyldendal.Porter.Application.Services.Imprint;

namespace Gyldendal.Porter.Application.HangfireJobs
{
    public class ImprintJob
    {
        private readonly IMediator _mediator;
        public ImprintJob(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Public Methods

        /// <summary>
        /// Kicks off updates for SubjectCode taxonomy 
        /// </summary>
        [DisableConcurrentExecution(int.MaxValue)]
        [AutomaticRetry(Attempts = 3, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public async Task Execute(PerformContext context)
        {
            await _mediator.Send(new ImprintUpdateCommand());
        }

        #endregion
    }
}
