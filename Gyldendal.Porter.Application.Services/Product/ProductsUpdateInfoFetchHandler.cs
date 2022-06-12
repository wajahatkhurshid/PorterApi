using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gyldendal.Porter.Application.Contracts.Enums;
using Gyldendal.Porter.Application.Contracts.Models;
using Gyldendal.Porter.Application.Contracts.Response;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using MediatR;

namespace Gyldendal.Porter.Application.Services.Product
{
    public class ProductsUpdateInfoFetchHandler : IRequestHandler<ProductsUpdateInfoFetchQuery, GetProductsUpdateInfoResponse>
    {
        private readonly ICookedProductRepository _cookedProductRepository;

        public ProductsUpdateInfoFetchHandler(ICookedProductRepository cookedProductRepository)
        {
            _cookedProductRepository = cookedProductRepository;
        }

        public async Task<GetProductsUpdateInfoResponse> Handle(ProductsUpdateInfoFetchQuery request, CancellationToken cancellationToken)
        {
            var updateAfterDateTime = new DateTime(request.ProductsUpdateInfoRequest.UpdatedAfterTicks, DateTimeKind.Utc);
            var products = await _cookedProductRepository.GetProductUpdatedInfoAsync(request.ProductsUpdateInfoRequest.WebShop, updateAfterDateTime, request.ProductsUpdateInfoRequest.PageIndex > 0 ? request.ProductsUpdateInfoRequest.PageIndex : 1,
                    request.ProductsUpdateInfoRequest.PageSize);

            var productUpdateInfos = products.Select(x => new ProductUpdateInfo
            {
                Id = x.Id,
                UpdateTime = x.UpdatedTimestamp,
                IsDeleted = x.IsDeleted,
                ProductType = (ProductType)Enum.Parse(typeof(ProductType), x.ProductType)
            }).ToList();

            return new GetProductsUpdateInfoResponse { ProductUpdateInfos = productUpdateInfos };
        }
    }
}
