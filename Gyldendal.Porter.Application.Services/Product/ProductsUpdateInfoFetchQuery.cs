using Gyldendal.Porter.Application.Contracts.Response;
using Gyldendal.Porter.Application.Contracts.Request;
using MediatR;

namespace Gyldendal.Porter.Application.Services.Product
{
    public class ProductsUpdateInfoFetchQuery : IRequest<GetProductsUpdateInfoResponse>
    {
        public GetProductsUpdateInfoRequest ProductsUpdateInfoRequest { get; set; }

        public ProductsUpdateInfoFetchQuery(GetProductsUpdateInfoRequest productsUpdateInfoRequest)
        {
            ProductsUpdateInfoRequest = productsUpdateInfoRequest;
        }
    }
}