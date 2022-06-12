using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Gyldendal.Porter.SolrMonitoring.Product.Models;

namespace Gyldendal.Porter.SolrMonitoring.Contributor
{
    public class ContributorMonitoringService
    {
        private static string _solrUrl = string.Empty;
        private static string _shopIds = string.Empty;
        public static async Task<ComparisonResult> CompareContributors(SolrMonitoringRequest criteria, string solrUrl, string shopIds)
        {
            _solrUrl = solrUrl;
            _shopIds = shopIds;
            var result = new ComparisonResult();

            var comparisonResults = new List<ShopComparisonResult>();
            var httpClient = new HttpClient();
            var webShops = new Dictionary<int, string>();

            var coreResult = await CreateCoreComparisonResult(-1, "Total contributors count comparison", httpClient);

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
                var comparisonResult = await CreateShopComparisonResult(webShop.Key, webShop.Value, httpClient, criteria);
                comparisonResults.Add(comparisonResult);

            }
            result.CoreComparison = coreResult;
            result.ShopsComparison = comparisonResults;
            return result;

        }

        private static async Task<CoreComparisonResult> CreateCoreComparisonResult(int webShopId, string comparisonName,
           HttpClient httpClient)
        {
            var count = 2;
            var clause = "*&indent=on&q=*:*&wt=json";

            var originalCoreQueryString =
                $"{_solrUrl}/contributors/select?fl=contributorid&fq={clause}";

            var shadowCoreQueryString = $"{_solrUrl}/porter_contributors/select?fl=contributorid&fq={clause}";
            //    $"{solrUrl}/porter_products/select?fq={clause}";
            var contriubtorsCoreResult = await Helpers.GetContributorSolrQueryResult(originalCoreQueryString, httpClient);
            var shadowContriubtorsCoreResult = await Helpers.GetContributorSolrQueryResult(shadowCoreQueryString, httpClient);

            var contributorsCount = contriubtorsCoreResult.response.numFound;
            var shadowContributorsCount= shadowContriubtorsCoreResult.response.numFound;
            
            var result = new CoreComparisonResult();
            result.Name = comparisonName;
            result.OriginalCoreCount = contributorsCount;
            result.ShadowCoreCount = shadowContributorsCount;
            result.Difference = contributorsCount - shadowContributorsCount;
            return result;
        }
        private static async Task<ShopComparisonResult> CreateShopComparisonResult(int webShopId, string comparisonName,
           HttpClient httpClient, SolrMonitoringRequest criteria)
        {
            var count = 2;
            var clause ="";
            clause = !string.IsNullOrWhiteSpace(criteria.Id)
                ? $"websiteId:{webShopId}&fq=contributorid:{criteria.Id}&indent=on&q=*:*&rows={count}&wt=json"
                : $"websiteId:{webShopId}&indent=on&q=*:*&rows={count}&wt=json";

            var originalCoreQueryString =
                $"{_solrUrl}/contributors/select?fl=contributorid&fq={clause}";

            var shadowCoreQueryString = $"{_solrUrl}/porter_contributors/select?fl=contributorid&fq={clause}";
            //    $"{solrUrl}/porter_products/select?fq={clause}";

            var contributorsCoreCountResult = await Helpers.GetContributorSolrQueryResult(originalCoreQueryString, httpClient);
            var shadowContributorsCoreCountResult = await Helpers.GetContributorSolrQueryResult(shadowCoreQueryString, httpClient);

            var totalOriginalContributors = contributorsCoreCountResult.response.numFound;
            var totalShadowContributors = shadowContributorsCoreCountResult.response.numFound;
            var batch = 100;
            var pageCount = totalOriginalContributors / batch;
            var remainder = totalOriginalContributors % batch;
            if (remainder > 0)
                pageCount += 1;
            var originalContributors = new List<Models.Contributor>();
            var shadowContributors = new List<Models.Contributor>();
            var startIndex = 0;
            for (var i = 0; i < 1; i++)
            {
                clause = !string.IsNullOrWhiteSpace(criteria.Id)
                    ? $"websiteId:{webShopId}&fq=contributorid:{criteria.Id}&indent=on&q=*:*&rows={batch}&start={startIndex}&wt=json"
                    : $"websiteId:{webShopId}&indent=on&q=*:*&rows={batch}&start={startIndex}&wt=json";

                originalCoreQueryString =
                    $"{_solrUrl}/contributors/select?fq={clause}";

                var contributorsCoreResult = await Helpers.GetContributorSolrQueryResult(originalCoreQueryString, httpClient);
                originalContributors.AddRange(contributorsCoreResult.response.docs);
                startIndex += batch;
            }
            pageCount = totalShadowContributors / batch;
            remainder = totalShadowContributors % batch;
            if (remainder > 0)
                pageCount += 1;
            startIndex = 0;
            for (var i = 0; i < 1; i++)
            {
                clause = !string.IsNullOrWhiteSpace(criteria.Id)
                    ? $"websiteId:{webShopId}&fq=contributorid:{criteria.Id}&indent=on&q=*:*&rows={batch}&wt=json"
                    : $"websiteId:{webShopId}&indent=on&q=*:*&rows={batch}&wt=json";

                shadowCoreQueryString = $"{_solrUrl}/porter_contributors/select?fq={clause}";
                var shadowContributorsCoreResult = await Helpers.GetContributorSolrQueryResult(shadowCoreQueryString, httpClient);
                shadowContributors.AddRange(shadowContributorsCoreResult.response.docs);
                startIndex += batch;
            }
            var contributorsCount = originalContributors.Count;
            var shadowContributorsCount = shadowContributors.Count;
            var result = new ShopComparisonResult();
            result.Name = comparisonName;
            result.OriginalCoreCount = contributorsCount;
            result.ShadowCoreCount = shadowContributorsCount;
            result.Difference = contributorsCount - shadowContributorsCount;
            if (webShopId >= 0)
            {
                var originalContributorIds = originalContributors.Select(r => r.contributorid).Distinct().ToList();
                var shadowContributorIds = shadowContributors.Select(r => r.contributorid).Distinct().ToList();
                var notFoundInShadow = originalContributorIds.Except(shadowContributorIds)
                    .ToList();
                var notFoundInOriginal = shadowContributorIds.Except(originalContributorIds)
                    .ToList();

                result.NotFoundInOriginal = String.Join(',', notFoundInOriginal);
                result.NotFoundInShadow = String.Join(',', notFoundInShadow);
                var originalPropsDifference = new List<PropertyDifference>();
                var shadowPropsDifference = new List<PropertyDifference>();

                originalContributors.ForEach(r =>
                    originalPropsDifference.Add(
                        new PropertyDifference()
                        {
                            Key = r.contributorid,
                            PropertiesDifferentInOriginal = Helpers.PublicPropertiesEqualityComperor(r, shadowContributors.FirstOrDefault(x => x.contributorid == r.contributorid))
                        }

                ));

                //result.PropertyDifferenceInOriginal = originalPropsDifference;
                shadowContributors.ForEach(r =>
                shadowPropsDifference.Add(
                        new PropertyDifference()
                        {
                            Key = r.contributorid,
                            PropertiesDifferentInShadow = Helpers.PublicPropertiesEqualityComperor(r, originalContributors.FirstOrDefault(x => x.contributorid == r.contributorid))
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
