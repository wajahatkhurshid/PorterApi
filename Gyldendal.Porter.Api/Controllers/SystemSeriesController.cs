using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Gyldendal.Porter.Application.Contracts.Request;
using Gyldendal.Porter.Application.Contracts.Response;
using Gyldendal.Porter.Application.Services.SystemSeries;

namespace Gyldendal.Porter.Api.Controllers
{
    /// <summary>
    ///  System Series Controller
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class SystemSeriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        public SystemSeriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get System Series By Id
        /// </summary>
        /// <returns>200 if message acknowledged</returns>
        [HttpPost]
        [ProducesResponseType(typeof(GetSeriesResponse), StatusCodes.Status200OK)]
        [Route("api/v1/SystemSeries/SeriesById/")]
        public async Task<ActionResult> GetSeriesById(GetSeriesRequest request)
        {
            var systemSeries = await _mediator.Send(new SeriesFetchQuery(request.WebShop,request.SeriesId));
            return Ok(systemSeries);
        }

        /// <summary>
        /// Get System Series Collection
        /// </summary>
        /// <returns>200 if message acknowledged</returns>
        [HttpPost]
        [ProducesResponseType(typeof(GetSeriesPaginatedResponse), StatusCodes.Status200OK)]
        [Route("api/v1/SystemSeries/Series/")]
        public async Task<ActionResult> GetSeries(GetSeriesPaginatedRequest request)
        {
            var systemSeries = await _mediator.Send(new SeriesPaginatedFetchQuery(request));
            return Ok(systemSeries);
        }
    }
}