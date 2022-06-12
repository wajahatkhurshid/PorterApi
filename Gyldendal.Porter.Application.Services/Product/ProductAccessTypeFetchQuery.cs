using Gyldendal.Porter.Application.Contracts.Request;
using Gyldendal.Porter.Application.Contracts.Response;
using MediatR;

namespace Gyldendal.Porter.Application.Services.Product
{
    public class ProductAccessTypeFetchQuery : IRequest<GetProductAccessTypeResponse>
    {
        public GetProductAccessTypeRequest ProductAccessTypeRequest { get; set; }

        public ProductAccessTypeFetchQuery(GetProductAccessTypeRequest productAccessTypeRequest)
        {
            ProductAccessTypeRequest = productAccessTypeRequest;
        }
    }
}