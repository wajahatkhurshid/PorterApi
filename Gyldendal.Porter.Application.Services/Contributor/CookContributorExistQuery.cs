using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Gyldendal.Porter.Domain.Contracts.Repositories;

namespace Gyldendal.Porter.Application.Services.Contributor
{
    public class CookContributorExistQuery : IRequest<bool>
    {
        public class CookProductExistQueryHandler : IRequestHandler<CookContributorExistQuery, bool>
        {
            private readonly ICookedContributorRepository _cookedContributorRepository;

            public CookProductExistQueryHandler(ICookedContributorRepository cookedContributorRepository)
            {
                _cookedContributorRepository = cookedContributorRepository;
            }

            public async Task<bool> Handle(CookContributorExistQuery request, CancellationToken cancellationToken)
            {
                var result = await _cookedContributorRepository.IsContributorCollectionExists();

                return result;
            }
        }
    }
}
