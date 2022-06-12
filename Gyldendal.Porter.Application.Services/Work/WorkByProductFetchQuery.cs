using System.Collections.Generic;
using System.Linq;
using Gyldendal.Porter.Application.Contracts.Response;
using Gyldendal.Porter.Application.Contracts.Request;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using DomainModels = Gyldendal.Porter.Domain.Contracts.Entities;
using ApplicationModels = Gyldendal.Porter.Application.Contracts.Models;
using AutoMapper;
using Gyldendal.Porter.Domain.Contracts.Entities.Cooked;

namespace Gyldendal.Porter.Application.Services.Work
{
    public class WorkByProductFetchQuery : IRequest<GetWorkByProductResponse>
    {
        public GetWorkByProductRequest WorkByProductRequest { get; set; }

        public WorkByProductFetchQuery(GetWorkByProductRequest workByProductRequest)
        {
            WorkByProductRequest = workByProductRequest;
        }

        public class WorkByProductFetchHandler : IRequestHandler<WorkByProductFetchQuery, GetWorkByProductResponse>
        {
            private readonly ICookedProductRepository _productRepository;
            private readonly IWorkRepository _workRepository;
            private readonly IMapper _mapper;

            public WorkByProductFetchHandler(ICookedProductRepository productRepository, IWorkRepository workRepository,
                IMapper mapper)
            {
                _productRepository = productRepository;
                _workRepository = workRepository;
                _mapper = mapper;
            }

            public async Task<GetWorkByProductResponse> Handle(WorkByProductFetchQuery request, CancellationToken cancellationToken)
            {
                var result = await _productRepository.SearchForAsync(x => x.Isbn == request.WorkByProductRequest.Isbn && x.WebShops.Contains(request.WorkByProductRequest.WebShop));
                var product = result.FirstOrDefault();
                if (product == null || !product.WorkId.HasValue)
                    return new GetWorkByProductResponse { Work = null };

                var work = await _workRepository.GetWorkByContainerInstanceIdAsync(product.WorkId.Value);

                return work == null ? new GetWorkByProductResponse { Work = null } : GetMappedWorkResponse(work, product);
            }

            private GetWorkByProductResponse GetMappedWorkResponse(DomainModels.Work work, CookedProduct cookedProduct)
            {
                var mappedWork = _mapper.Map<ApplicationModels.Work>(work);
                mappedWork.Products = new List<ApplicationModels.Product>();

                var applicationProduct = _mapper.Map<ApplicationModels.Product>(cookedProduct);
                applicationProduct.SubjectCodes = _mapper.Map<List<ApplicationModels.SubjectCode>>(cookedProduct.SubjectCodes);

                mappedWork.Products.Add(applicationProduct);
                return new GetWorkByProductResponse
                {
                    Work = mappedWork,
                };
            }
        }
    }
}