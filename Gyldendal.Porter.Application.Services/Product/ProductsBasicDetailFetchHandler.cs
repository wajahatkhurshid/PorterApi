using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ApplicationModels = Gyldendal.Porter.Application.Contracts.Models;
using Gyldendal.Porter.Application.Contracts.Response;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using MediatR;

namespace Gyldendal.Porter.Application.Services.Product
{
    public class ProductsBasicDetailFetchHandler : IRequestHandler<ProductsBasicDetailFetchQuery, GetProductsBasicDetailResponse>
    {
        private IMapper _mapper;
        private readonly ICookedProductRepository _cookedProductRepository;

        public ProductsBasicDetailFetchHandler(ICookedProductRepository cookedProductRepository, IMapper mapper)
        {
            _mapper = mapper;
            _cookedProductRepository = cookedProductRepository;
        }
        public async Task<GetProductsBasicDetailResponse> Handle(ProductsBasicDetailFetchQuery request, CancellationToken cancellationToken)
        {
            var products = await _cookedProductRepository.GetListAsync(request.LicensedProductsRequest.Isbns, request.LicensedProductsRequest.WebShop);

            return new GetProductsBasicDetailResponse { ProductBasicDetails = _mapper.Map<List<ApplicationModels.ProductBasicDetail>>(products) };
        }
    }
}