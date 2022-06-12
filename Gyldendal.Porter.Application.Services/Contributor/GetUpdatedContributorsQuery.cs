using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Gyldendal.Porter.Application.Contracts.Enums;
using Gyldendal.Porter.Domain.Contracts.Repositories;

namespace Gyldendal.Porter.Application.Services.Contributor
{
    public class GetUpdatedContributorsQuery : IRequest<int>
    {
        public WebShop WebShop;

        public long UpdatedAfterTicks { get; set; }

        public class GetUpdateContributorQueryHandler : IRequestHandler<GetUpdatedContributorsQuery, int>
        {
            private ICookedContributorRepository ContributorRepository { get; set; }

            public GetUpdateContributorQueryHandler(ICookedContributorRepository contributorRepository)
            {
                ContributorRepository = contributorRepository;
            }

            public async Task<int> Handle(GetUpdatedContributorsQuery request, CancellationToken cancellationToken)
            {
                var updateAfterDateTime = new DateTime(request.UpdatedAfterTicks, DateTimeKind.Utc);
                var contributorCount = await ContributorRepository.SearchForCountAsync(x => x.WebShops.Contains(request.WebShop) && x.UpdatedTimestamp >= updateAfterDateTime);
                return (int)contributorCount;
            }
        }
    }
}
