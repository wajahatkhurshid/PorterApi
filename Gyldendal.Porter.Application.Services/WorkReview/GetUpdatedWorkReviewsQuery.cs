using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Gyldendal.Porter.Domain.Contracts.Repositories;

namespace Gyldendal.Porter.Application.Services.WorkReview
{
    public class GetUpdatedWorkReviewsQuery : IRequest<int>
    {
        public long UpdatedAfterTicks { get; set; }

        public class GetUpdateContributorQueryHandler : IRequestHandler<GetUpdatedWorkReviewsQuery, int>
        {
            private ICookedWorkReviewRepository WorkReviewRepository { get; set; }

            public GetUpdateContributorQueryHandler(ICookedWorkReviewRepository workReviewRepository)
            {
                WorkReviewRepository = workReviewRepository;
            }

            public async Task<int> Handle(GetUpdatedWorkReviewsQuery request, CancellationToken cancellationToken)
            {
                var updateAfterDateTime = new DateTime(request.UpdatedAfterTicks, DateTimeKind.Utc);
                var workReviews = await WorkReviewRepository.SearchForAsync(x => x.UpdatedTimestamp >= updateAfterDateTime);
                return workReviews.Count;
            }
        }
    }
}