using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Gyldendal.Porter.SolrMonitoring.Product.Models;

namespace Gyldendal.Porter.SolrMonitoring.Product
{
    public static class ProductMonitoringService
    {
        private static string _solrUrl = string.Empty;
        private static string _shopIds = string.Empty;
        public static async Task<ComparisonResult> CompareProducts(SolrMonitoringRequest criteria, string solrUrl,string shopIds)
        {
            _solrUrl = solrUrl;
            _shopIds = shopIds;
            var result = new ComparisonResult();

            var comparisonResults = new List<ShopComparisonResult>();
            var httpClient = new HttpClient();
            var webShops = new Dictionary<int, string>();

            var coreResult = await CreateCoreComparisonResult(-1, "Total product count comparison", httpClient);

            if (criteria.ShopId == 0) //All Shops
            {
                var shops = _shopIds.Split(',');
                foreach (var shop in shops)
                {
                    webShops.Add(Int32.Parse(shop), Enum.Parse(typeof(WebShop), shop).ToString());
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
                $"{_solrUrl}/products/select?fl=isbn13&fq={clause}";

            var shadowCoreQueryString = $"{_solrUrl}/porter_products/select?fl=isbn13&fq={clause}";
            //    $"{solrUrl}/porter_products/select?fq={clause}";
            var productsCoreResult = await Helpers.GetSolrQueryResult(originalCoreQueryString, httpClient);
            var shadowProductsCoreResult = await Helpers.GetSolrQueryResult(shadowCoreQueryString, httpClient);

            var productsCount = productsCoreResult.response.numFound;
            var shadowProductsFound = shadowProductsCoreResult.response.numFound;
            
            var result = new CoreComparisonResult();
            result.Name = comparisonName;
            result.OriginalCoreCount = productsCount;
            result.ShadowCoreCount = shadowProductsFound;
            result.Difference = productsCount - shadowProductsFound;
            return result;
        }
        private static async Task<ShopComparisonResult> CreateShopComparisonResult(int webShopId, string comparisonName,
           HttpClient httpClient, SolrMonitoringRequest criteria)
        {
            var count = 2;
            var clause = "";
            clause = !string.IsNullOrWhiteSpace(criteria.Id)
                ? $"websiteId:{webShopId}&fq=isbn13:{criteria.Id}&indent=on&q=*:*&rows={count}&wt=json"
                : $"websiteId:{webShopId}&indent=on&q=*:*&rows={count}&wt=json";

            var originalCoreQueryString =
                $"{_solrUrl}/products/select?fl=isbn13&fq={clause}";

            var shadowCoreQueryString = $"{_solrUrl}/porter_products/select?fl=isbn13&fq={clause}";
            //    $"{solrUrl}/porter_products/select?fq={clause}";

            var productsCoreCountResult = await Helpers.GetSolrQueryResult(originalCoreQueryString, httpClient);
            var shadowProductsCoreCountResult = await Helpers.GetSolrQueryResult(shadowCoreQueryString, httpClient);

            var totalOriginalProducts = productsCoreCountResult.response.numFound;
            var totalShadowProducts = shadowProductsCoreCountResult.response.numFound;
            var batch = 100;
            var pageCount = totalOriginalProducts / batch;
            var remainder = totalOriginalProducts % batch;
            if (remainder > 0)
                pageCount += 1;
            var originalProducts = new List<Models.Product>();
            var shadowProducts = new List<Models.Product>();
            var startIndex = 0;
            for (var i = 0; i < 1; i++)
            {
                clause = !string.IsNullOrWhiteSpace(criteria.Id)
                    ? $"websiteId:{webShopId}&fq=isbn13:{criteria.Id}&indent=on&q=*:*&rows={batch}&start={startIndex}&wt=json"
                    : $"websiteId:{webShopId}&indent=on&q=*:*&rows={batch}&start={startIndex}&wt=json";
                originalCoreQueryString =
                    $"{_solrUrl}/products/select?fq={clause}";

                var productsCoreResult = await Helpers.GetSolrQueryResult(originalCoreQueryString, httpClient);
                originalProducts.AddRange(productsCoreResult.response.docs);
                startIndex += batch;
            }
            pageCount = totalShadowProducts / batch;
            remainder = totalShadowProducts % batch;
            if (remainder > 0)
                pageCount += 1;
            startIndex = 0;
            for (var i = 0; i < 1; i++)
            {
                clause = !string.IsNullOrWhiteSpace(criteria.Id)
                    ? $"websiteId:{webShopId}&fq=isbn13:{criteria.Id}&indent=on&q=*:*&rows={batch}&wt=json"
                    : $"websiteId:{webShopId}&indent=on&q=*:*&rows={batch}&wt=json";
                shadowCoreQueryString = $"{_solrUrl}/porter_products/select?fq={clause}";
                var shadowProductsCoreResult = await Helpers.GetSolrQueryResult(shadowCoreQueryString, httpClient);
                shadowProducts.AddRange(shadowProductsCoreResult.response.docs);
                startIndex += batch;
            }
            var productsCount = originalProducts.Count;
            var shadowProductsFound = shadowProducts.Count;
            var result = new ShopComparisonResult();
            result.Name = comparisonName;
            result.OriginalCoreCount = productsCount;
            result.ShadowCoreCount = shadowProductsFound;
            result.Difference = productsCount - shadowProductsFound;
            if (webShopId >= 0)
            {
                var originalIsbns = originalProducts.Select(r => r.isbn13).Distinct().ToList();
                var shadowIsbns = shadowProducts.Select(r => r.isbn13).Distinct().ToList();
                var notFoundInShadow = originalIsbns.Except(shadowIsbns)
                    .ToList();
                var notFoundInOriginal = shadowIsbns.Except(originalIsbns)
                    .ToList();

                result.NotFoundInOriginal = String.Join(',', notFoundInOriginal);
                result.NotFoundInShadow = String.Join(',', notFoundInShadow);
                var originalPropsDifference = new List<PropertyDifference>();
                var shadowPropsDifference = new List<PropertyDifference>();

                originalProducts.ForEach(r =>
                    originalPropsDifference.Add(
                        new PropertyDifference()
                        {
                            Key = r.isbn13,
                            PropertiesDifferentInOriginal = Helpers.PublicPropertiesEqualityComperor(r, shadowProducts.FirstOrDefault(x => x.isbn13 == r.isbn13))
                        }

                ));

                //result.PropertyDifferenceInOriginal = originalPropsDifference;
                shadowProducts.ForEach(r =>
                shadowPropsDifference.Add(
                        new PropertyDifference()
                        {
                            Key = r.isbn13,
                            PropertiesDifferentInShadow = Helpers.PublicPropertiesEqualityComperor(r, originalProducts.FirstOrDefault(x => x.isbn13 == r.isbn13))
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
