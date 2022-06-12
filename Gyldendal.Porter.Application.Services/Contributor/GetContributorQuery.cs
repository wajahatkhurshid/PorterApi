using System.Threading;
using System.Threading.Tasks;
using Gyldendal.Porter.Application.Contracts.Enums;
using Gyldendal.Porter.Application.Contracts.Response;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using MediatR;

namespace Gyldendal.Porter.Application.Services.Contributor
{
    public class GetContributorQuery : IRequest<GetContributorResponse>
    {
        public string Id { get; set; }
        public WebShop WebShop { get; set; }

        public class GetContributorQueryHandler : IRequestHandler<GetContributorQuery, GetContributorResponse>
        {
            private readonly ICookedContributorRepository _contributorRepository;

            public GetContributorQueryHandler(ICookedContributorRepository contributorRepository)
            {
                _contributorRepository = contributorRepository;
            }

            public async Task<GetContributorResponse> Handle(GetContributorQuery request,
                CancellationToken cancellationToken)
            {
                var result = await _contributorRepository.GetContributorById(request.Id, request.WebShop);
                if (result == null)
                    return null;

                var response = new GetContributorResponse
                {
                    Contributor = new Contracts.Models.Contributor
                    {
                        Id = result.Id,
                        Bibliography = result.Bibliography,
                        UpdatedTimestamp = result.UpdatedTimestamp,
                        BiographyText = result.BiographyText,
                        LastName = result.LastName,
                        ContributorTypeId = result.ContributorTypeId,
                        FirstName = result.FirstName,
                        PhotoUrl = result.PhotoUrl,
                        WebShops = result.WebShops
                    }
                };

                return response;
            }
        }
    }
}