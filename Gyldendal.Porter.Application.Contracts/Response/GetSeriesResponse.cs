using Gyldendal.Porter.Application.Contracts.Models;
using Newtonsoft.Json;

namespace Gyldendal.Porter.Application.Contracts.Response
{
    public class GetSeriesResponse
    {
        [AllowNull]
        [JsonProperty("Series", Required = Required.AllowNull, DefaultValueHandling = DefaultValueHandling.Include)]
        public Series Series { get; set; }
    }
}
