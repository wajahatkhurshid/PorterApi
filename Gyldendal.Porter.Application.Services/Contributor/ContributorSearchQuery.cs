using AutoMapper;
using Gyldendal.Porter.Application.Contracts.Response;
using Gyldendal.Porter.Common.Request;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Gyldendal.Porter.Application.Services.Contributor
{
    public class ContributorSearchQuery : IRequest<ContributorSearchResponse>
    {
        public string SearchString { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string PropertiesToInclude { get; set; }
        public class GetProductQueryHandler : IRequestHandler<ContributorSearchQuery, ContributorSearchResponse>
        {
            private readonly ICookedContributorRepository _contributorRepository;
            private readonly IMapper _mapper;

            public GetProductQueryHandler(ICookedContributorRepository contributorRepository, IMapper mapper)
            {
                _contributorRepository = contributorRepository;
                _mapper = mapper;
            }
            public async Task<ContributorSearchResponse> Handle(ContributorSearchQuery request, CancellationToken cancellationToken)
            {

                var result = await _contributorRepository.ContributorSearchAsync(new SearchContributorRequest()
                {
                    SearchString = request.SearchString,
                    Page = request.Page ,
                    PageSize = request.PageSize,
                    PropertiesToInclude = request.PropertiesToInclude
                });
                return new ContributorSearchResponse()
                {
                    Results = result.Results.Select(x => _mapper.Map<Contracts.Models.Contributor>(x)).ToList(),
                    Count = result.Count
                };
            }
        }
    }
}
