using Gyldendal.Porter.Application.Contracts.Response;
using Gyldendal.Porter.Application.Contracts.Request;
using MediatR;

namespace Gyldendal.Porter.Application.Services.Product
{
    public class ProductDetailsFetchQuery : IRequest<GetProductDetailsResponse>
    {
        public GetProductDetailsRequest ProductDetailsRequest { get; set; }

        public ProductDetailsFetchQuery(GetProductDetailsRequest productDetailsRequest)
        {
            ProductDetailsRequest = productDetailsRequest;
        }
    }
}