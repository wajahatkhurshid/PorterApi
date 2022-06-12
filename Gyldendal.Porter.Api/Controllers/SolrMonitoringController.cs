using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gyldendal.Porter.Common.Configurations;
using Gyldendal.Porter.SolrMonitoring;
using Gyldendal.Porter.SolrMonitoring.Contributor;
using Microsoft.AspNetCore.Http;
using Gyldendal.Porter.SolrMonitoring.Product;
using Gyldendal.Porter.SolrMonitoring.WorkReviews;

namespace Gyldendal.Porter.Api.Controllers
{
    /// <summary>
    /// Contains functionality related to the monitoring and comparison of old SOLR cores with the new Porter cores
    /// </summary>
    [ApiController]
    public class SolrMonitoringController : ControllerBase
    {
        /// <summary>
        /// Criteria=> 0=All Shops
        /// 2=GU,3=HR,5=Munks,11=ClubBogklub,12=ClubBoerne,13=ClubSamleren,14=ClubKrimi,15=ClubPsykeSjael,16=ClubHistorie,17=ClubPaedagogisk,
        /// 18=Highlight,19=ClubBoerne3To5,20=ClubBoerne5To10,21=ClubFlamingo,22,23,24,26
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(List<ComparisonResult>), StatusCodes.Status200OK)]
        [Route("api/v1/SolrMonitoring/CompareProducts")]
        public async Task<IActionResult> CompareProducts(SolrMonitoringRequest criteria)
        {
            var solrUrl = AppConfigurations.Configuration.AppSettings.SolrUrl;
            var webShopsIds = AppConfigurations.Configuration.AppSettings.SolrShopIds;
            var result = await ProductMonitoringService.CompareProducts(criteria, solrUrl, webShopsIds);
            return Ok(result);
        }


        /// <summary>
        /// Criteria=> 0=All Shops
        /// 2=GU,3=HR,5=Munks,11=ClubBogklub,12=ClubBoerne,13=ClubSamleren,14=ClubKrimi,15=ClubPsykeSjael,16=ClubHistorie,17=ClubPaedagogisk,
        /// 18=Highlight,19=ClubBoerne3To5,20=ClubBoerne5To10,21=ClubFlamingo,22,23,24,26
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(List<ComparisonResult>), StatusCodes.Status200OK)]
        [Route("api/v1/SolrMonitoring/CompareContributors")]
        public async Task<IActionResult> CompareContributors(SolrMonitoringRequest criteria)
        {
            var solrUrl = AppConfigurations.Configuration.AppSettings.SolrUrl;
            var webShopsIds = AppConfigurations.Configuration.AppSettings.SolrShopIds;
            var result = await ContributorMonitoringService.CompareContributors(criteria, solrUrl, webShopsIds);
            return Ok(result);
        }

        /// <summary>
        /// Criteria=> 0=All Shops
        /// 2=GU,3=HR,5=Munks,11=ClubBogklub,12=ClubBoerne,13=ClubSamleren,14=ClubKrimi,15=ClubPsykeSjael,16=ClubHistorie,17=ClubPaedagogisk,
        /// 18=Highlight,19=ClubBoerne3To5,20=ClubBoerne5To10,21=ClubFlamingo,22,23,24,26
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(List<ComparisonResult>), StatusCodes.Status200OK)]
        [Route("api/v1/SolrMonitoring/CompareWorkReviews")]
        public async Task<IActionResult> CompareWorkReviews(SolrMonitoringRequest criteria)
        {
            var solrUrl = AppConfigurations.Configuration.AppSettings.SolrUrl;
            var webShopsIds = AppConfigurations.Configuration.AppSettings.SolrShopIds;
            var result = await WorkReviewsMonitoringService.CompareWorkReviews(criteria, solrUrl, webShopsIds);
            return Ok(result);
        }
    }
}