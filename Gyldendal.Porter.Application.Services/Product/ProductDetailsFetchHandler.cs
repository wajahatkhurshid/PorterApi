using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ApplicationModels = Gyldendal.Porter.Application.Contracts.Models;
using Gyldendal.Porter.Application.Contracts.Response;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using DomainModels = Gyldendal.Porter.Domain.Contracts.Entities;
using MediatR;

namespace Gyldendal.Porter.Application.Services.Product
{
    public class ProductDetailsFetchQueryHandler : IRequestHandler<ProductDetailsFetchQuery, GetProductDetailsResponse>
    {
        private readonly IMapper _mapper;
        private readonly ICookedProductRepository _cookedProductRepository;

        public ProductDetailsFetchQueryHandler(IMapper mapper, ICookedProductRepository cookedProductRepository)
        {
            _mapper = mapper;
            _cookedProductRepository = cookedProductRepository;
        }

        public async Task<GetProductDetailsResponse> Handle(ProductDetailsFetchQuery request, CancellationToken cancellationToken)
        {
            var product = await _cookedProductRepository.GetProductDetailsAsync(request.ProductDetailsRequest.Isbn, request.ProductDetailsRequest.ProductType, request.ProductDetailsRequest.WebShop);

            return GetMappedProductResponse(product);
        }

        private GetProductDetailsResponse GetMappedProductResponse(DomainModels.Cooked.CookedProduct product)
        {
            var productDetailsResponse = new GetProductDetailsResponse
            {
                Product = _mapper.Map<ApplicationModels.Product>(product)
            };

            return productDetailsResponse;
        }
    }
}