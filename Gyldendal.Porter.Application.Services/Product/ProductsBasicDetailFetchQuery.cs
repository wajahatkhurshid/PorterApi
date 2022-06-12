using Gyldendal.Porter.Application.Contracts.Response;
using Gyldendal.Porter.Application.Contracts.Request;
using MediatR;

namespace Gyldendal.Porter.Application.Services.Product
{
    public class ProductsBasicDetailFetchQuery : IRequest<GetProductsBasicDetailResponse>
    {
        public GetProductsBasicDetailRequest LicensedProductsRequest { get; set; }

        public ProductsBasicDetailFetchQuery(GetProductsBasicDetailRequest licensedProductsRequest)
        {
            LicensedProductsRequest = licensedProductsRequest;
        }
    }
}