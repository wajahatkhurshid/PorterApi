using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Gyldendal.Porter.Application.Contracts.Request;
using Gyldendal.Porter.Application.Contracts.Response;
using Gyldendal.Porter.Application.Services.Work;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Gyldendal.Porter.Api.Controllers
{
    /// <summary>
    ///  Work Controller
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class WorkController : Controller
    {
        private readonly IMediator _mediator;

        public WorkController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets a work with full details
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(GetWorkResponse), StatusCodes.Status200OK)]
        [Route("api/v1/Work/Get")]
        public async Task<ActionResult> GetWork(GetWorkQuery request)
        {
            var work = await _mediator.Send(request);
            return Ok(work);
        }

        /// <summary>
        /// Gets a work by product Id and type
        /// </summary>
        /// <returns>200 if message acknowledged</returns>
        [HttpPost]
        [ProducesResponseType(typeof(GetWorkByProductResponse), StatusCodes.Status200OK)]
        [Route("api/v1/Work/GetByProduct")]
        public async Task<ActionResult> GetWorkByProduct(GetWorkByProductRequest request)
        {
            var work = await _mediator.Send(new WorkByProductFetchQuery(request));
            return Ok(work);
        }
    }
}
