using System.Threading;
using System.Threading.Tasks;
using Gyldendal.Porter.Application.Contracts.Enums;
using Gyldendal.Porter.Application.Contracts.Response;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using MediatR;

namespace Gyldendal.Porter.Application.Services.Contributor
{
    public class GetContributorUpdateInfoQuery : IRequest<GetContributorsUpdateInfoResponse>
    {
        public WebShop WebShop { get; set; }

        public string ContributorId { get; set; }

        public class GetUpdateContributorQueryHandler : IRequestHandler<GetContributorUpdateInfoQuery, GetContributorsUpdateInfoResponse>
        {
            private ICookedContributorRepository ContributorRepository { get; set; }

            public GetUpdateContributorQueryHandler(ICookedContributorRepository contributorRepository)
            {
                ContributorRepository = contributorRepository;
            }

            public async Task<GetContributorsUpdateInfoResponse> Handle(GetContributorUpdateInfoQuery request, CancellationToken cancellationToken)
            {
                var contributor = await ContributorRepository.GetContributorById(request.ContributorId, request.WebShop);

                if (contributor == null)
                {
                    return new GetContributorsUpdateInfoResponse();
                }

                return new GetContributorsUpdateInfoResponse
                {
                    Id = contributor.Id,
                    UpdateTime = contributor.UpdatedTimestamp,
                    IsDeleted = contributor.IsDeleted
                };
            }
        }
    }
}
