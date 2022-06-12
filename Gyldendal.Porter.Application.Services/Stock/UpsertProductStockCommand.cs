using System;
using System.Threading;
using System.Threading.Tasks;
using Gyldendal.Porter.Application.Services.EntityProcessing;
using Gyldendal.Porter.Domain.Contracts.Entities;
using Gyldendal.Porter.Domain.Contracts.Entities.Queue;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using MediatR;

namespace Gyldendal.Porter.Application.Services.Stock
{
    public class UpsertProductStockCommand : IRequest<bool>
    {
        public string Isbn { get; set; }
        public int AvailableStock { get; set; }

        public UpsertProductStockCommand(string isbn, int availableStock)
        {
            Isbn = isbn;
            AvailableStock = availableStock;
        }

        public class UpsertProductStockCommandHandler : IRequestHandler<UpsertProductStockCommand, bool>
        {
            private readonly IProductStockRepository _productStockRepository;
            private readonly IMediator _mediator;

            public UpsertProductStockCommandHandler(IProductStockRepository productStockRepository, IMediator mediator)
            {
                _productStockRepository = productStockRepository;
                _mediator = mediator;
            }

            public async Task<bool> Handle(UpsertProductStockCommand request, CancellationToken cancellationToken)
            {
                var product = await _productStockRepository.GetProductStockByIsbnAsync(request.Isbn) ?? new ProductStock
                {
                    Id = Guid.NewGuid().ToString(),
                    Isbn = request.Isbn,
                };

                if (product.AvailableStock != request.AvailableStock)
                {
                    product.AvailableStock = Convert.ToInt32(request.AvailableStock);
                    await _productStockRepository.UpsertProductStockAsync(product);
                    await _mediator.Publish(new EntityUpdateReceivedNotification(EntityType.Product, request.Isbn), cancellationToken);
                }

                return true;
            }
        }

    }
}
