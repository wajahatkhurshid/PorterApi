using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Gyldendal.Porter.Application.Contracts.Response;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using AutoMapper;
using Gyldendal.Porter.Application.Contracts.Enums;

namespace Gyldendal.Porter.Application.Services.WorkReview
{
    public class GetWorkReviewQuery : IRequest<GetWorkReviewResponse>
    {
        public string Id { get; set; }
        public WebShop WebShop { get; set; }

        public class GetWorkQueryHandler : IRequestHandler<GetWorkReviewQuery, GetWorkReviewResponse>
        {
            private readonly ICookedWorkReviewRepository _workReviewRepository;
            private readonly IMapper _mapper;

            public GetWorkQueryHandler(ICookedWorkReviewRepository workReviewRepository, IMapper mapper)
            {
                _workReviewRepository = workReviewRepository;
                _mapper = mapper;
            }
            public async Task<GetWorkReviewResponse> Handle(GetWorkReviewQuery request, CancellationToken cancellationToken)
            {
                var result = await _workReviewRepository.GetByIdAsync(request.Id);
                return new GetWorkReviewResponse { WorkReview = _mapper.Map<Contracts.Models.WorkReview>(result) };
            }
        }
    }
}
