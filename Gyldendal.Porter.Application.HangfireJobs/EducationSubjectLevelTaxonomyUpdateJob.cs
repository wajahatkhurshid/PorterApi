using System.Threading.Tasks;
using Gyldendal.Porter.Application.Services.EducationSubjectLevel;
using Hangfire;
using Hangfire.Server;
using MediatR;

namespace Gyldendal.Porter.Application.HangfireJobs
{
    public class EducationSubjectLevelTaxonomyUpdateJob
    {
        private readonly IMediator _mediator;

        public EducationSubjectLevelTaxonomyUpdateJob(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Manages updates for Internet Categories
        /// </summary>
        [DisableConcurrentExecution(int.MaxValue)] //50+ Years
        [AutomaticRetry(Attempts = 3, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public async Task Execute(PerformContext context)
        {
            await _mediator.Send(new EducationSubjectLevelUpdateCommand());
        }
    }
}
