using Gyldendal.Porter.Application.Contracts.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using MediatR;

namespace Gyldendal.Porter.Application.Services.WorkReview
{
    public class GetWorkReviewsUpdateInfoQuery : IRequest<List<GetWorkReviewUpdateInfoResponse>>, IRequest<GetContributorsUpdateInfoResponse>
    {
        public long UpdatedAfterTicks { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public class GetUpdateWorkReviewQueryHandler : IRequestHandler<GetWorkReviewsUpdateInfoQuery, List<GetWorkReviewUpdateInfoResponse>>
        {
            private ICookedWorkReviewRepository WorkReviewRepository { get; set; }

            public GetUpdateWorkReviewQueryHandler(ICookedWorkReviewRepository workReviewRepository)
            {
                WorkReviewRepository = workReviewRepository;
            }

            public async Task<List<GetWorkReviewUpdateInfoResponse>> Handle(GetWorkReviewsUpdateInfoQuery request, CancellationToken cancellationToken)
            {
                var updateAfterDateTime = new DateTime(request.UpdatedAfterTicks, DateTimeKind.Utc);
                var contributors = await WorkReviewRepository.GetByPaginationAsync(x => x.UpdatedTimestamp >= updateAfterDateTime, request.PageSize, request.PageIndex == 0 ? 1 : request.PageIndex);

                return contributors.Select(x => new GetWorkReviewUpdateInfoResponse
                {
                    WorkReviewId = x.Id,
                    UpdateTime = x.UpdatedTimestamp,
                    UpdateType = x.IsDeleted
                }).ToList();
            }
        }
    }
}
