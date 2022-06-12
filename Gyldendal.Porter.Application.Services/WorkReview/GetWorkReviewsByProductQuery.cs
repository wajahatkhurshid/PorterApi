using System.Collections.Generic;
using System.Linq;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using AutoMapper;
using Gyldendal.Porter.Application.Contracts.Enums;

namespace Gyldendal.Porter.Application.Services.WorkReview
{
    public class GetWorkReviewsByProductQuery : IRequest<List<Contracts.Models.WorkReview>>
    {
        public string Isbn { get; set; }
        public WebShop WebShop { get; set; }

        public class GetWorkReviewsByProductQueryHandler : IRequestHandler<GetWorkReviewsByProductQuery, List<Contracts.Models.WorkReview>>
        {
            private readonly ICookedProductRepository _cookedProductRepository;
            private readonly ICookedWorkReviewRepository _workReviewRepository;
            private readonly IMapper _mapper;

            public GetWorkReviewsByProductQueryHandler(ICookedWorkReviewRepository workReviewRepository, IMapper mapper, ICookedProductRepository cookedProductRepository)
            {
                _workReviewRepository = workReviewRepository;
                _mapper = mapper;
                _cookedProductRepository = cookedProductRepository;
            }

            public async Task<List<Contracts.Models.WorkReview>> Handle(GetWorkReviewsByProductQuery request, CancellationToken cancellationToken)
            {
                var cookedProduct = await _cookedProductRepository.GetProductDetailsAsync(request.Isbn, request.WebShop);
                if (cookedProduct == null)
                    return new List<Contracts.Models.WorkReview>();

                var cookedWorkReviews = await _workReviewRepository.SearchForAsync(x => x.WorkId == cookedProduct.WorkId);
                if (cookedWorkReviews == null || cookedWorkReviews.Count == 0)
                    return new List<Contracts.Models.WorkReview>();

                var workReviews = cookedWorkReviews.Select(review => _mapper.Map<Contracts.Models.WorkReview>(review)).ToList();
                return workReviews;
            }
        }
    }
}
