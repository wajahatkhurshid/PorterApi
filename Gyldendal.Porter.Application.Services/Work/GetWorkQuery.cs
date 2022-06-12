using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Gyldendal.Porter.Application.Contracts.Enums;
using Gyldendal.Porter.Application.Contracts.Response;
using Gyldendal.Porter.Common;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using MediatR;

namespace Gyldendal.Porter.Application.Services.Work
{
    public class GetWorkQuery : IRequest<GetWorkResponse>
    {
        public string Isbn { get; set; }
        public WebShop WebShop { get; set; }

        public class GetWorkQueryHandler : IRequestHandler<GetWorkQuery, GetWorkResponse>
        {
            private readonly IWorkRepository _workRepository;
            private readonly ICookedProductRepository _cookedProductRepository;
            private readonly IMapper _mapper;
            private readonly ILogger _logger;

            public GetWorkQueryHandler(IWorkRepository workRepository, ICookedProductRepository cookedProductRepository,
                IMapper mapper, ILogger logger)
            {
                _workRepository = workRepository;
                _cookedProductRepository = cookedProductRepository;
                _mapper = mapper;
                _logger = logger;
            }

            public async Task<GetWorkResponse> Handle(GetWorkQuery request, CancellationToken cancellationToken)
            {
                var product = await _cookedProductRepository.GetByIdAsync(request.Isbn);

                if (!(product is { WorkId: { } }))
                {
                    _logger.Info("No work ID found for product", isGdprSafe: true);
                    return new GetWorkResponse();
                }

                var work = await _workRepository.GetWorkByContainerInstanceIdAsync(product.WorkId.Value);
                var products = await _cookedProductRepository.SearchForAsync(p => p.WorkId == work.ContainerInstanceId);

                var workResponse = _mapper.Map<Contracts.Models.Work>(work);
                workResponse.Products = _mapper.Map<List<Contracts.Models.Product>>(products);
                var response = new GetWorkResponse
                {
                    Work = workResponse
                };

                return response;
            }
        }
    }
}