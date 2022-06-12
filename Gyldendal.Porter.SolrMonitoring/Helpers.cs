using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Gyldendal.Porter.SolrMonitoring.Contributor.Models;
using Gyldendal.Porter.SolrMonitoring.Product.Models;
using Gyldendal.Porter.SolrMonitoring.WorkReviews.Models;

namespace Gyldendal.Porter.SolrMonitoring
{
    public static class Helpers
    {
        public static Dictionary<string, string> PublicPropertiesEqualityComperor<T>(T self, T to) where T : class
        {
            var missMatchedProps = new Dictionary<string, string>();
            if (self != null && to != null)
            {
                var type = typeof(T);
                foreach (var pi in type.GetProperties(System.Reflection.BindingFlags.Public |
                    System.Reflection.BindingFlags.Instance))
                {
                    var selfValue = type.GetProperty(pi.Name)?.GetValue(self, null);
                    var toValue = type.GetProperty(pi.Name)?.GetValue(to, null);
                    if (pi.PropertyType.FullName == "System.String[]")
                    {
                        if (selfValue == null && toValue != null || selfValue != null && toValue == null)
                        {
                            Console.WriteLine("Mismatch with property '{0}.{1}' found.", type.FullName, pi.Name);
                            missMatchedProps.Add(pi.Name, $"Property is null");
                        }
                        else if (selfValue != null && toValue != null)
                        {
                            var collectionItems1 = ((IEnumerable) selfValue).Cast<object>();
                            var collectionItems2 = ((IEnumerable) toValue).Cast<object>();
                            var collectionItemsCount1 = collectionItems1.Count();
                            var collectionItemsCount2 = collectionItems2.Count();
                            // check the counts to ensure they match
                            if (collectionItemsCount1 != collectionItemsCount2)
                            {
                                Console.WriteLine("Collection counts for property '{0}.{1}' do not match.",
                                    type.FullName, pi.Name);

                                missMatchedProps.Add(pi.Name, $"Count mismatched: {collectionItemsCount1}");
                            }
                            // and if they do, compare each item... this assumes both collections have the same order
                            else
                            {
                                for (var i = 0; i < collectionItemsCount1; i++)
                                {
                                    var collectionItem1 = collectionItems1.ElementAt(i);
                                    var collectionItem2 = collectionItems2.ElementAt(i);
                                    if (collectionItem1 != collectionItem2 && (collectionItem1 == null || !collectionItem1.Equals(collectionItem2)))
                                    {
                                        missMatchedProps.Add(pi.Name, collectionItem1?.ToString());

                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (selfValue != toValue && (selfValue == null || !selfValue.Equals(toValue)))
                        {
                            missMatchedProps.Add(pi.Name, selfValue?.ToString());
                        }
                    }
                }
            }
            return missMatchedProps;
        }

        public static async Task<SolrProductQueryResponseBody> GetSolrQueryResult(string queryUrl, HttpClient httpClient)
        {
            var response = await httpClient.GetAsync(queryUrl);
            var responseBody = await response.Content.ReadAsStringAsync();
            var solrResult = JsonSerializer.Deserialize<SolrProductQueryResponseBody>(responseBody);
            return solrResult;
        }

        public static async Task<SolrContributorQueryResponseBody> GetContributorSolrQueryResult(string queryUrl, HttpClient httpClient)
        {
            var response = await httpClient.GetAsync(queryUrl);
            var responseBody = await response.Content.ReadAsStringAsync();
            var solrResult = JsonSerializer.Deserialize<SolrContributorQueryResponseBody>(responseBody);
            return solrResult;
        }

        public static async Task<SolrWorkReviewQueryResponseBody> GetWorkReviewsSolrQueryResult(string queryUrl, HttpClient httpClient)
        {
            var response = await httpClient.GetAsync(queryUrl);
            var responseBody = await response.Content.ReadAsStringAsync();
            var solrResult = JsonSerializer.Deserialize<SolrWorkReviewQueryResponseBody>(responseBody);
            return solrResult;
        }
    }
}
