using Gyldendal.Porter.Application.Contracts.Request;
using Gyldendal.Porter.Application.Contracts.Response;
using MediatR;

namespace Gyldendal.Porter.Application.Services.Product
{
    public class ProductAttachmentsFetchQuery : IRequest<GetProductAttachmentsResponse>
    {
        public GetProductAttachmentsRequest ProductAttachmentsRequest { get; set; }

        public ProductAttachmentsFetchQuery(GetProductAttachmentsRequest productAttachmentsRequest)
        {
            ProductAttachmentsRequest = productAttachmentsRequest;
        }
    }
}