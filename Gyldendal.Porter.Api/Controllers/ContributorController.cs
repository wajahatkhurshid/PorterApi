using System.Collections.Generic;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Gyldendal.Porter.Application.Contracts.Enums;
using Gyldendal.Porter.Application.Contracts.Response;
using Gyldendal.Porter.Application.Services.Contributor;

namespace Gyldendal.Porter.Api.Controllers
{
    /// <summary>
    ///  Contributor Controller
    /// </summary>
    [ApiController]
    public class ContributorController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Endpoints for fetching contributor change sets and look up of contributors
        /// </summary>
        public ContributorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets a Contributor
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(GetContributorResponse), StatusCodes.Status200OK)]
        [Route("api/v1/Contributor/Get")]
        public async Task<ActionResult> GetContributor(GetContributorQuery request)
        {
            var contributor = await _mediator.Send(request);
            return Ok(contributor);
        }


        /// <summary>
        /// Returns the number of contributors updated after the given DateTime.
        /// </summary>
        /// <param name="webShop"></param>
        /// <param name="updatedAfterTicks"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [Route("api/v1/Contributor/GetUpdatedContributorsCount")]
        public async Task<ActionResult> GetUpdatedContributorsCount(WebShop webShop, long updatedAfterTicks)
        {
            return Ok(await _mediator.Send(new GetUpdatedContributorsQuery { WebShop = webShop,  UpdatedAfterTicks = updatedAfterTicks }));
        }

        /// <summary>
        /// Returns the asked page of ContributorUpdatedInfo objects, for each contributor
        /// </summary>
        /// <param name="webShop"></param>
        /// <param name="updatedAfterTicks">Timestamp to indicate the time a contributor has been updated after</param>
        /// <param name="pageIndex">Minimum value 1.</param>
        /// <param name="pageSize">Minimum value 1.</param>
        /// <returns>A list of contributors update information(updated, deleted, what time)</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<GetContributorsUpdateInfoResponse>), StatusCodes.Status200OK)]
        [Route("api/v1/Contributor/GetContributorsUpdateInfo")]
        public async Task<ActionResult> GetContributorsUpdateInfo(WebShop webShop, long updatedAfterTicks, int pageIndex, int pageSize)
        {
            return Ok(await _mediator.Send(new GetContributorsUpdateInfoQuery
            { WebShop = webShop, UpdatedAfterTicks = updatedAfterTicks, PageIndex = pageIndex, PageSize = pageSize }));
        }

        /// <summary>
        /// Returns the latest update info for synchronization purposes, for the given contributor id
        /// </summary>
        /// <param name="webShop"></param>
        /// <param name="contributorId">ID of the contributor</param>
        /// <returns>Returns the update info for a specific contributor ID</returns>
        [HttpGet]
        [Route("api/v1/Contributor/GetContributorUpdateInfo")]
        [ProducesResponseType(typeof(GetContributorsUpdateInfoResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetContributorUpdateInfo(WebShop webShop, string contributorId)
        {
            return Ok(await _mediator.Send(new GetContributorUpdateInfoQuery { WebShop = webShop, ContributorId = contributorId }));
        }

        /// <summary>
        /// Get Author by search string(first name)
        /// </summary>
        /// <returns>A list of contributors and their count</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ContributorSearchResponse), StatusCodes.Status200OK)]
        [Route("api/v1/Contributor/ContributorSearch")]
        public async Task<ActionResult> ContributorSearch(ContributorSearchQuery request)
        {
            return Ok(await _mediator.Send(request));
        }

        /// <summary>
        /// Checks that Contributor cooked Collection Exists and its document size is greater than 0
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [Route("api/v1/Contributor/IsContributorDataAvailable")]
        public async Task<ActionResult> IsContributorDataAvailable()
        {
            return Ok(await _mediator.Send(new CookContributorExistQuery()));
        }
    }
}