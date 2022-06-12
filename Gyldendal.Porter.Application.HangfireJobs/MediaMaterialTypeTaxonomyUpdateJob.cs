using Hangfire;
using Hangfire.Server;
using System.Threading.Tasks;
using MediatR;
using Gyldendal.Porter.Application.Services.MediaMaterialType;

namespace Gyldendal.Porter.Application.HangfireJobs
{
    public class MediaMaterialTypeTaxonomyUpdateJob
    {
        private readonly IMediator _mediator;

        public MediaMaterialTypeTaxonomyUpdateJob(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Kicks off updates for media- and material types
        /// </summary>
        [DisableConcurrentExecution(int.MaxValue)] //50+ Years
        [AutomaticRetry(Attempts = 3, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public async Task Execute(PerformContext context)
        {
            await _mediator.Send(new MediaMaterialTypeUpdateCommand());
        }
    }
}