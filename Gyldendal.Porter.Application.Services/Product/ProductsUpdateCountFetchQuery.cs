using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using Gyldendal.Porter.Application.Contracts.Request;

namespace Gyldendal.Porter.Application.Services.Product
{
    public class ProductsUpdateCountFetchQuery : IRequest<int>
    {
        public GetProductsUpdateCountRequest ProductsUpdatedCountRequest;
        public ProductsUpdateCountFetchQuery(GetProductsUpdateCountRequest productsUpdatedCountRequest)
        {
            ProductsUpdatedCountRequest = productsUpdatedCountRequest;
        }

        public class GetUpdateProductsCountQueryHandler : IRequestHandler<ProductsUpdateCountFetchQuery, int>
        {
            private readonly ICookedProductRepository _cookedProductRepository;

            public GetUpdateProductsCountQueryHandler(ICookedProductRepository cookedProductRepository)
            {
                _cookedProductRepository = cookedProductRepository;
            }

            public async Task<int> Handle(ProductsUpdateCountFetchQuery request, CancellationToken cancellationToken)
            {
                var updateAfterDateTime = new DateTime(request.ProductsUpdatedCountRequest.UpdatedAfterTicks, DateTimeKind.Utc);
                var productsCount = await _cookedProductRepository.GetUpdatedCount(request.ProductsUpdatedCountRequest.WebShop, updateAfterDateTime);
                return productsCount;
            }
        }
    }
}
