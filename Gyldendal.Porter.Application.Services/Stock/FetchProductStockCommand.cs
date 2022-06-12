using System.Threading;
using System.Threading.Tasks;
using Gyldendal.Porter.Domain.Contracts.Interfaces;
using MediatR;

namespace Gyldendal.Porter.Application.Services.Stock
{
    public class FetchProductStockCommand : IRequest<bool>
    {
        public string Isbn { get; set; }

        public FetchProductStockCommand(string isbn)
        {
            Isbn = isbn;
        }

        public class FetchProductStockCommandHandler : IRequestHandler<FetchProductStockCommand, bool>
        {
            private readonly IProductStockClient _productStockClient;
            private readonly IMediator _mediator;

            public FetchProductStockCommandHandler(IProductStockClient productStockClient, IMediator mediator)
            {
                _productStockClient = productStockClient;
                _mediator = mediator;
            }

            public async Task<bool> Handle(FetchProductStockCommand request, CancellationToken cancellationToken)
            {
                var stock = await _productStockClient.FetchAvailableStockAsync(request.Isbn);
                var command = new UpsertProductStockCommand(request.Isbn, stock);
                return await _mediator.Send(command, cancellationToken);
            }
        }
    }
}
