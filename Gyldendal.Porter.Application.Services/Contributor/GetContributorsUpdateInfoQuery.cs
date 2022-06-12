using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gyldendal.Porter.Application.Contracts.Enums;
using Gyldendal.Porter.Application.Contracts.Response;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using MediatR;

namespace Gyldendal.Porter.Application.Services.Contributor
{
    public class GetContributorsUpdateInfoQuery : IRequest<List<GetContributorsUpdateInfoResponse>>, IRequest<GetContributorsUpdateInfoResponse>
    {
        public WebShop WebShop { get; set; }

        public long UpdatedAfterTicks { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public class GetUpdateContributorQueryHandler : IRequestHandler<GetContributorsUpdateInfoQuery, List<GetContributorsUpdateInfoResponse>>
        {
            private ICookedContributorRepository ContributorRepository { get; set; }

            public GetUpdateContributorQueryHandler(ICookedContributorRepository contributorRepository)
            {
                ContributorRepository = contributorRepository;
            }

            public async Task<List<GetContributorsUpdateInfoResponse>> Handle(GetContributorsUpdateInfoQuery request, CancellationToken cancellationToken)
            {
                var updateAfterDateTime = new DateTime(request.UpdatedAfterTicks, DateTimeKind.Utc);
                var contributors = await ContributorRepository.GetByPaginationAsync(
                    x => x.WebShops.Contains(request.WebShop) && x.UpdatedTimestamp >= updateAfterDateTime, request.PageSize, request.PageIndex == 0 ? 1 : request.PageIndex);

                return contributors.Select(x => new GetContributorsUpdateInfoResponse
                {
                    Id = x.Id,
                    UpdateTime = x.UpdatedTimestamp,
                    IsDeleted = x.IsDeleted
                }).ToList();

            }
        }
    }
}
