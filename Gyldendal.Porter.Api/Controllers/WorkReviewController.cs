using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Gyldendal.Porter.Application.Contracts.Response;
using Gyldendal.Porter.Application.Services.WorkReview;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Gyldendal.Porter.Api.Controllers
{
    /// <summary>
    ///  WorkReview Controller
    /// </summary>
    [ApiController]
    public class WorkReviewController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        public WorkReviewController(IMediator mediator)
        {
            _mediator = mediator;
        }


        /// <summary>
        /// Get WorkReview
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(GetWorkReviewResponse), StatusCodes.Status200OK)]
        [Route("api/v1/WorkReview/Get")]
        public async Task<ActionResult> GetWorkReview(GetWorkReviewQuery request)
        {
            var workReview = await _mediator.Send(request);
            return Ok(workReview);
        }

        /// <summary>
        /// Returns the asked page of WorkReviewUpdatedInfo objects, for each WorkReview
        /// </summary>
        /// <param name="updatedAfterTicks"></param>
        /// <param name="pageIndex">Minimum value 1.</param>
        /// <param name="pageSize">Minimum value 1.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<GetWorkReviewUpdateInfoResponse>), StatusCodes.Status200OK)]
        [Route("api/v1/WorkReview/GetWorkReviewsUpdateInfo")]
        public async Task<ActionResult> GetWorkReviewsUpdateInfo(long updatedAfterTicks, int pageIndex, int pageSize)
        {
            return Ok(await _mediator.Send(new GetWorkReviewsUpdateInfoQuery { UpdatedAfterTicks = updatedAfterTicks, PageIndex = pageIndex, PageSize = pageSize }));
        }


        /// <summary>
        /// Returns the number of WorkReviews updated after the given DateTime.
        /// </summary>
        /// <param name="updatedAfterTicks"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [Route("api/v1/WorkReview/GetUpdatedWorkReviewsCount")]
        public async Task<ActionResult> GetUpdatedWorkReviewsCount(long updatedAfterTicks)
        {
            return Ok(await _mediator.Send(new GetUpdatedWorkReviewsQuery { UpdatedAfterTicks = updatedAfterTicks }));
        }

        /// <summary>
        /// Checks that WorkReview Collection Exists and its document size is greater than 0
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [Route("api/v1/WorkReview/IsWorkReviewDataAvailable")]
        public async Task<ActionResult> IsWorkReviewDataAvailable()
        {
            return Ok(await _mediator.Send(new CookWorkReviewExistQuery()));
        }

        /// <summary>
        /// Get WorkReviews against Product
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(List<Application.Contracts.Models.WorkReview>), StatusCodes.Status200OK)]
        [Route("api/v1/WorkReviews/GetWorkReviews")]
        public async Task<ActionResult> GetWorkReviews(GetWorkReviewsByProductQuery request)
        {
            var workReview = await _mediator.Send(request);
            return Ok(workReview);
        }
    }
}
