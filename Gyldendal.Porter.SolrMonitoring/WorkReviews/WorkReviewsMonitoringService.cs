using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Gyldendal.Porter.SolrMonitoring.Product.Models;

namespace Gyldendal.Porter.SolrMonitoring.WorkReviews
{

    public static class WorkReviewsMonitoringService
    {
        private static string _solrUrl = string.Empty;
        private static string _shopIds = string.Empty;

        public static async Task<ComparisonResult> CompareWorkReviews(SolrMonitoringRequest criteria, string solrUrl, string shopIds)
        {
            _solrUrl = solrUrl;
            _shopIds = shopIds;
            var result = new ComparisonResult();

            var comparisonResults = new List<ShopComparisonResult>();
            var httpClient = new HttpClient();
            var webShops = new Dictionary<int, string>();

            var coreResult = await CreateCoreComparisonResult(-1, "Total WorkReviews count comparison", httpClient);

            if (criteria.ShopId == 0) //All Shops
            {
                var shops = _shopIds.Split(',');
                foreach (var shop in shops)
                {
                    webShops.Add(int.Parse(shop), Enum.Parse(typeof(WebShop), shop).ToString());
                }
            }
            else
            {
                webShops.Add(criteria.ShopId, ((WebShop)criteria.ShopId).ToString());
            }

            foreach (var webShop in webShops)
            {
                var comparisonResult = await CreateShopComparisonResult(webShop.Key, webShop.Value, httpClient,criteria);
                comparisonResults.Add(comparisonResult);

            }
            result.CoreComparison = coreResult;
            result.ShopsComparison = comparisonResults;
            return result;

        }
        private static async Task<CoreComparisonResult> CreateCoreComparisonResult(int webShopId, string comparisonName,HttpClient httpClient)
        {
            var count = 2;
            var clause = "*&indent=on&q=*:*&wt=json";

            var originalCoreQueryString =
                $"{_solrUrl}/workreviews/select?fl=workReviewId&fq={clause}";

            var shadowCoreQueryString = $"{_solrUrl}/porter_workreviews/select?fl=workReviewId&fq={clause}";
            //    $"{solrUrl}/porter_products/select?fq={clause}";
            var workReviewsCoreResult = await Helpers.GetWorkReviewsSolrQueryResult(originalCoreQueryString, httpClient);
            var shadowWorkReviewsCoreResult = await Helpers.GetWorkReviewsSolrQueryResult(shadowCoreQueryString, httpClient);

            var workReviewsCount = workReviewsCoreResult.response.numFound;
            var shadowWorkReviewsCount = shadowWorkReviewsCoreResult.response.numFound;

            var result = new CoreComparisonResult();
            result.Name = comparisonName;
            result.OriginalCoreCount = workReviewsCount;
            result.ShadowCoreCount = shadowWorkReviewsCount;
            result.Difference = workReviewsCount - shadowWorkReviewsCount;
            return result;
        }
        private static async Task<ShopComparisonResult> CreateShopComparisonResult(int webShopId, string comparisonName,HttpClient httpClient,SolrMonitoringRequest criteria)
        {
            var count = 2;
            var clause = "";
            clause = !string.IsNullOrWhiteSpace(criteria.Id)
                ? $"workReviewId:{criteria.Id}&fq=websiteId:{webShopId}&indent=on&q=*:*&rows={count}&wt=json"
                : $"websiteId:{webShopId}&indent=on&q=*:*&rows={count}&wt=json";
            var originalCoreQueryString =
                $"{_solrUrl}/workreviews/select?fl=workReviewId&fq={clause}";

            var shadowCoreQueryString = $"{_solrUrl}/porter_workreviews/select?fl=workReviewId&fq={clause}";
            //    $"{solrUrl}/porter_products/select?fq={clause}";

            var workReviewsCoreCountResult = await Helpers.GetWorkReviewsSolrQueryResult(originalCoreQueryString, httpClient);
            var shadowWorkReviewsCoreCountResult = await Helpers.GetWorkReviewsSolrQueryResult(shadowCoreQueryString, httpClient);

            var totalOriginalWorkReviews = workReviewsCoreCountResult.response.numFound;
            var totalShadowWorkReviews = shadowWorkReviewsCoreCountResult.response.numFound;
            var batch = 100;
            var pageCount = totalOriginalWorkReviews / batch;
            var remainder = totalOriginalWorkReviews % batch;
            if (remainder > 0)
                pageCount += 1;
            var originalWorkReviews = new List<Models.WorkReview>();
            var shadowWorkReviews = new List<Models.WorkReview>();
            var startIndex = 0;
            for (var i = 0; i < 1; i++)
            {

                clause = !string.IsNullOrWhiteSpace(criteria.Id)
                    ? $"workReviewId:{criteria.Id}&fq=websiteId:{webShopId}&indent=on&q=*:*&rows={batch}&start={startIndex}&wt=json"
                    : $"websiteId:{webShopId}&indent=on&q=*:*&rows={batch}&start={startIndex}&wt=json";

                originalCoreQueryString =
                    $"{_solrUrl}/workreviews/select?fq={clause}";

                var workReviewsCoreResult = await Helpers.GetWorkReviewsSolrQueryResult(originalCoreQueryString, httpClient);
                originalWorkReviews.AddRange(workReviewsCoreResult.response.docs);
                startIndex += batch;
            }
            pageCount = totalShadowWorkReviews / batch;
            remainder = totalShadowWorkReviews % batch;
            if (remainder > 0)
                pageCount += 1;
            startIndex = 0;
            for (var i = 0; i < 1; i++)
            {
                clause = !string.IsNullOrWhiteSpace(criteria.Id)
                    ? $"workReviewId:{criteria.Id}&fq=websiteId:{webShopId}&indent=on&q=*:*&rows={batch}&wt=json"
                    : $"websiteId:{webShopId}&indent=on&q=*:*&rows={batch}&wt=json";
                shadowCoreQueryString = $"{_solrUrl}/porter_workreviews/select?fq={clause}";
                var shadowWorkReviewsCoreResult = await Helpers.GetWorkReviewsSolrQueryResult(shadowCoreQueryString, httpClient);
                shadowWorkReviews.AddRange(shadowWorkReviewsCoreResult.response.docs);
                startIndex += batch;
            }
            var workReviewsCount = originalWorkReviews.Count;
            var shadowWorkReviewsCount = shadowWorkReviews.Count;
            var result = new ShopComparisonResult();
            result.Name = comparisonName;
            result.OriginalCoreCount = workReviewsCount;
            result.ShadowCoreCount = shadowWorkReviewsCount;
            result.Difference = workReviewsCount - shadowWorkReviewsCount;
            if (webShopId >= 0)
            {
                var originalWorkReviewsIds = originalWorkReviews.Select(r => r.workReviewId).Distinct().ToList();
                var shadowWorkReviewsIds = shadowWorkReviews.Select(r => r.workReviewId).Distinct().ToList();
                var notFoundInShadow = originalWorkReviewsIds.Except(shadowWorkReviewsIds)
                    .ToList();
                var notFoundInOriginal = shadowWorkReviewsIds.Except(originalWorkReviewsIds)
                    .ToList();

                result.NotFoundInOriginal = string.Join(',', notFoundInOriginal);
                result.NotFoundInShadow = string.Join(',', notFoundInShadow);
                var originalPropsDifference = new List<PropertyDifference>();
                var shadowPropsDifference = new List<PropertyDifference>();

                originalWorkReviews.ForEach(r =>
                    originalPropsDifference.Add(
                        new PropertyDifference()
                        {
                            Key = r.workReviewId.ToString(),
                            PropertiesDifferentInOriginal = Helpers.PublicPropertiesEqualityComperor(r, shadowWorkReviews.FirstOrDefault(x => x.workReviewId == r.workReviewId))
                        }

                ));

                //result.PropertyDifferenceInOriginal = originalPropsDifference;
                shadowWorkReviews.ForEach(r =>
                shadowPropsDifference.Add(
                        new PropertyDifference()
                        {
                            Key = r.workReviewId.ToString(),
                            PropertiesDifferentInShadow = Helpers.PublicPropertiesEqualityComperor(r, originalWorkReviews.FirstOrDefault(x => x.workReviewId == r.workReviewId))
                        }

                    ));
                var difference = originalPropsDifference.Join(shadowPropsDifference, o => o.Key, s => s.Key,
                    (o, s) => new
                    {
                        original = o,
                        shadow = s
                    }).Select(x => new PropertyDifference()
                    {
                        Key = x.original.Key,
                        PropertiesDifferentInOriginal = x.original.PropertiesDifferentInOriginal,
                        PropertiesDifferentInShadow = x.shadow.PropertiesDifferentInShadow
                    }).ToList();
                result.ItemsDifference = difference.Where(r => r.PropertiesDifferentInShadow.Any() && r.PropertiesDifferentInOriginal.Any()).ToList();
            }

            return result;
        }
    }
}
