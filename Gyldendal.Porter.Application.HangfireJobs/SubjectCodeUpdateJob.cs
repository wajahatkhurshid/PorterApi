using Hangfire;
using MediatR;
using System.Threading.Tasks;
using Gyldendal.Porter.Application.Services.SubjectCode;
using Hangfire.Server;

namespace Gyldendal.Porter.Application.HangfireJobs
{
   public class SubjectCodeUpdateJob
    {
        private readonly IMediator _mediator;

        public SubjectCodeUpdateJob(IMediator mediator)
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
            await _mediator.Send(new SubjectCodeUpdateCommand());
        }

        #endregion
    }
}
