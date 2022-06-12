using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Gyldendal.Porter.Application.Contracts.Request;
using Gyldendal.Porter.Application.Contracts.Response;
using Gyldendal.Porter.Application.Services.Product;

namespace Gyldendal.Porter.Api.Controllers
{
    /// <summary>
    ///  Product Controller
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get Product Details
        /// </summary>
        /// <returns>200 if message acknowledged</returns>
        [HttpPost]
        [ProducesResponseType(typeof(GetProductDetailsResponse), StatusCodes.Status200OK)]
        [Route("api/v1/Product/Details")]
        public async Task<ActionResult> GetDetails(GetProductDetailsRequest request)
        {
            var product = await _mediator.Send(new ProductDetailsFetchQuery(request));
            return Ok(product);
        }

        /// <summary>
        /// Get Product Access Type
        /// </summary>
        /// <returns>200 if message acknowledged</returns>
        [HttpPost]
        [ProducesResponseType(typeof(GetProductAccessTypeResponse), StatusCodes.Status200OK)]
        [Route("api/v1/Product/AccessType")]
        public async Task<ActionResult> GetAccessType(GetProductAccessTypeRequest request)
        {
            var product = await _mediator.Send(new ProductAccessTypeFetchQuery(request));
            return Ok(product);
        }

        /// <summary>
        /// Get Products Basic Detail
        /// </summary>
        /// <returns>200 if message acknowledged</returns>
        [HttpPost]
        [ProducesResponseType(typeof(GetProductsBasicDetailResponse), StatusCodes.Status200OK)]
        [Route("api/v1/Product/BasicDetails")]
        public async Task<ActionResult> GetBasicDetail(GetProductsBasicDetailRequest request)
        {
            var product = await _mediator.Send(new ProductsBasicDetailFetchQuery(request));
            return Ok(product);
        }

        /// <summary>
        /// Get Product Attachments
        /// </summary>
        /// <returns>200 if message acknowledged</returns>
        [HttpPost]
        [ProducesResponseType(typeof(GetProductAttachmentsResponse), StatusCodes.Status200OK)]
        [Route("api/v1/Product/Attachments")]
        public async Task<ActionResult> GetAttachments(GetProductAttachmentsRequest request)
        {
            var attachments = await _mediator.Send(new ProductAttachmentsFetchQuery(request));
            return Ok(attachments);
        }

        /// <summary>
        /// Get count of Products updated for a web shop after the given time.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [Route("api/v1/Product/GetUpdateCount")]
        public async Task<ActionResult> GetUpdatedCount(GetProductsUpdateCountRequest request)
        {
            return Ok(await _mediator.Send(new ProductsUpdateCountFetchQuery(request)));
        }

        /// <summary>
        /// Get Products update info
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(GetProductsUpdateInfoResponse), StatusCodes.Status200OK)]
        [Route("api/v1/Product/GetUpdateInfo")]
        public async Task<ActionResult> GetUpdatedInfo(GetProductsUpdateInfoRequest request)
        {
            return Ok(await _mediator.Send(new ProductsUpdateInfoFetchQuery(request)));
        }

        /// <summary>
        /// Get Search Products by Isbn starting with provided value
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(SearchProductResponse), StatusCodes.Status200OK)]
        [Route("api/v1/Product/ProductSearch")]
        public async Task<ActionResult> ProductSearch(ProductSearchQuery request)
        {
            return Ok(await _mediator.Send(request));
        }

        /// <summary>
        /// Checks that Product Collection Exists and its document size is greater than 0
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [Route("api/v1/Product/IsProductDataAvailable")]
        public async Task<ActionResult> IsProductDataAvailable()
        {
            return Ok(await _mediator.Send(new CookProductExistQuery()));
        }
    }
}