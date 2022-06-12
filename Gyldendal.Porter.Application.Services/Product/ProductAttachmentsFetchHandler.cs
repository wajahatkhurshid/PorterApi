using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gyldendal.Porter.Application.Contracts.Models;
using Gyldendal.Porter.Application.Contracts.Response;
using Gyldendal.Porter.Domain.Contracts.Entities.Cooked;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using MediatR;

namespace Gyldendal.Porter.Application.Services.Product
{
    public class ProductAttachmentsFetchHandler : IRequestHandler<ProductAttachmentsFetchQuery, GetProductAttachmentsResponse>
    {
        private readonly ICookedProductRepository _cookedProductRepository;

        public ProductAttachmentsFetchHandler(ICookedProductRepository cookedProductRepository)
        {
            _cookedProductRepository = cookedProductRepository;
        }

        public async Task<GetProductAttachmentsResponse> Handle(ProductAttachmentsFetchQuery request, CancellationToken cancellationToken)
        {
            var product = await _cookedProductRepository.GetProductByIdAsync(request.ProductAttachmentsRequest.Isbn);

            return new GetProductAttachmentsResponse() { Attachments = GetAttachments(product) };
        }


        private List<Attachment> GetAttachments(CookedProduct product)
        {
            if (product.Attachments.Any())
            {
                return product.Attachments.Select(x => new Attachment()
                {
                    Description = x.AttachmentDescription,
                    SampleUrl = x.AttachmentTitle,
                    PublizonIdentifier = x.AttachmentFileType,
                    IsDeleted = false, // todo
                    IsSecured = true // todo
                }).ToList();
            }
            return null;
        }
    }
}