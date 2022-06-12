using System.Threading.Tasks;
using Gyldendal.Porter.Application.Services.InternetCategory;
using Hangfire;
using Hangfire.Server;
using MediatR;

namespace Gyldendal.Porter.Application.HangfireJobs
{
    public class InternetCategoryTaxonomyUpdateJob
    {
        private readonly IMediator _mediator;

        public InternetCategoryTaxonomyUpdateJob(IMediator mediator)
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
            await _mediator.Send(new InternetCategoryUpdateCommand());
        }
    }
}
