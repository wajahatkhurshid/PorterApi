using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Gyldendal.Porter.Domain.Contracts.Repositories;

namespace Gyldendal.Porter.Application.Services.WorkReview
{
    public class CookWorkReviewExistQuery : IRequest<bool>
    {
        public class CookProductExistQueryHandler : IRequestHandler<CookWorkReviewExistQuery, bool>
        {
            private readonly ICookedWorkReviewRepository _cookedWorkReviewRepository;

            public CookProductExistQueryHandler(ICookedWorkReviewRepository cookedWorkReviewRepository)
            {
                _cookedWorkReviewRepository = cookedWorkReviewRepository;
            }

            public async Task<bool> Handle(CookWorkReviewExistQuery request, CancellationToken cancellationToken)
            {
                var result = await _cookedWorkReviewRepository.IsWorkReviewCollectionExists();

                return result;
            }
        }
    }
}