using Hangfire;
using Hangfire.Server;
using System.Threading.Tasks;
using Gyldendal.Porter.Application.Services.Stock;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using MediatR;

namespace Gyldendal.Porter.Application.HangfireJobs
{
    public class ProductStockUpdateJob
    {
        private readonly IProductRepository _productRepository;
        private readonly IMediator _mediator;

        public ProductStockUpdateJob(IProductRepository productRepository, IMediator mediator)
        {
            _productRepository = productRepository;
            _mediator = mediator;
        }
        
        /// <summary>
        /// this is the core method which executes at the startup of project,
        /// it calls relevant listener to process events and trigger next child job
        /// to keep this process always in running mode until project stops.
        /// </summary>
        /// <param name="context">the object which carries jobId</param>
        /// <returns></returns>
        [DisableConcurrentExecution(int.MaxValue)]//50+ Years
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public async Task Execute(PerformContext context)
        {
            var products = await _productRepository.GetProductsAsync();

            foreach (var product in products)
            {
                var command = new FetchProductStockCommand(product.Isbn);
                await _mediator.Send(command);
            }
        }
    }
}
