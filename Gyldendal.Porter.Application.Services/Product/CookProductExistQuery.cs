using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Gyldendal.Porter.Domain.Contracts.Repositories;

namespace Gyldendal.Porter.Application.Services.Product
{
    public class CookProductExistQuery : IRequest<bool>
    {
        public class CookProductExistQueryHandler : IRequestHandler<CookProductExistQuery, bool>
        {
            private readonly ICookedProductRepository _cookedProductRepository;

            public CookProductExistQueryHandler(ICookedProductRepository cookedProductRepository)
            {
                _cookedProductRepository = cookedProductRepository;
            }

            public async Task<bool> Handle(CookProductExistQuery request, CancellationToken cancellationToken)
            {
                var result = await _cookedProductRepository.IsProductCollectionExists();

                return result;
            }
        }
    }
}