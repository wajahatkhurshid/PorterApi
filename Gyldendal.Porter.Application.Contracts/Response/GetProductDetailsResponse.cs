using Gyldendal.Porter.Application.Contracts.Models;
using Newtonsoft.Json;

namespace Gyldendal.Porter.Application.Contracts.Response
{
    public class GetProductDetailsResponse
    {
        [AllowNull]
        [JsonProperty("Product", Required = Required.AllowNull, DefaultValueHandling = DefaultValueHandling.Include)]
        public Product Product { get; set; }
    }
}
