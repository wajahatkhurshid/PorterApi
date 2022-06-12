using System.Threading.Tasks;
using Gyldendal.Porter.Application.Contracts.Enums;
using Gyldendal.Porter.Application.Services.MediaMaterialType;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Gyldendal.Porter.Application.Contracts.Request;
using Gyldendal.Porter.Application.Contracts.Response;
using Gyldendal.Porter.Application.Services.InternetCategory;
using Gyldendal.Porter.Application.Services.EducationSubjectLevel;

namespace Gyldendal.Porter.Api.Controllers
{
    /// <summary>
    ///  Master Data Controller
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class MasterDataController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        public MasterDataController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get collection of Media Types
        /// </summary>
        /// <returns>200 if message acknowledged</returns>
        [HttpGet]
        [ProducesResponseType(typeof(GetMediaTypesResponse), StatusCodes.Status200OK)]
        [Route("api/v1/MasterData/MediaTypes")]
        public async Task<ActionResult> GetMediaTypes()
        {
            var mediaTypes = await _mediator.Send(new MediaTypeFetchQuery());
            return Ok(mediaTypes);
        }

        /// <summary>
        /// Get collection of Material Types
        /// </summary>
        /// <returns>200 if message acknowledged</returns>
        [HttpGet]
        [ProducesResponseType(typeof(GetMaterialTypesResponse), StatusCodes.Status200OK)]
        [Route("api/v1/MasterData/MaterialTypes")]
        public async Task<ActionResult> GetMaterialTypes()
        {
            var materialTypes = await _mediator.Send(new MaterialTypeFetchQuery());
            return Ok(materialTypes);
        }

        /// <summary>
        /// Get a collection of Areas by webshop
        /// </summary>
        /// <param name="webShop"></param>
        /// <returns>200 if message acknowledged</returns>
        [HttpGet]
        [ProducesResponseType(typeof(GetAreasResponse), StatusCodes.Status200OK)]
        [Route("api/v1/MasterData/Areas/{webShop}")]
        public async Task<ActionResult> GetAreas(WebShop webShop)
        {
            var areas = await _mediator.Send(new AreaFetchQuery(webShop));
            return Ok(areas);
        }

        /// <summary>
        ///  Get a collection of Subjects for the specified Webshop and for Specific Area
        /// (Source: MongoDB)
        /// </summary>
        /// <param name="request">Webshop, AreaId</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(GetSubjectsResponse), StatusCodes.Status200OK)]
        [Route("api/v1/PorterMasterData/Subjects/")]
        public async Task<ActionResult> GetSubjects(GetSubjectsRequest request)
        {
            var subjects = await _mediator.Send(new SubjectFetchQuery(request));
            return Ok(subjects);
        }

        /// <summary>
        ///  Get a collection of SubAreas for the specified Webshop and for Subject
        /// </summary>
        /// <param name="request">Webshop,SubjectId</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(GetSubAreasResponse), StatusCodes.Status200OK)]
        [Route("api/v1/PorterMasterData/SubAreas/")]
        public async Task<ActionResult> GetSubAreas(GetSubAreasRequest request)
        {
            var subAreas = await _mediator.Send(new SubAreaFetchQuery(request));
            return Ok(subAreas);
        }

        /// <summary>
        ///  Get a collection of Levels for the specified Webshop and for Specific Area
        /// </summary>
        /// <param name="request">Webshop,AreaId</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(GetLevelsResponse), StatusCodes.Status200OK)]
        [Route("api/v1/MasterData/Levels/")]
        public async Task<ActionResult> GetLevels(GetLevelsRequest request)
        {
            var levels = await _mediator.Send(new LevelFetchQuery(request));
            return Ok(levels);
        }
    }
}