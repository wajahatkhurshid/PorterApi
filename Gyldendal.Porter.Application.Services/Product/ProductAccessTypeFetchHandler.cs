using System.Threading;
using System.Threading.Tasks;
using Gyldendal.Porter.Application.Contracts.Response;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using MediatR;

namespace Gyldendal.Porter.Application.Services.Product
{
    public class ProductAccessTypeFetchQueryHandler : IRequestHandler<ProductAccessTypeFetchQuery, GetProductAccessTypeResponse>
    {
        private readonly IProductRepository _productRepository;

        public ProductAccessTypeFetchQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<GetProductAccessTypeResponse> Handle(ProductAccessTypeFetchQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetProductByIdAsync(request.ProductAccessTypeRequest.Isbn);

            //TODO: product is missing the accessControl property on Mongo and Domain Models...

            return new GetProductAccessTypeResponse { AccessControl = "Ekey" };
        }
    }
}