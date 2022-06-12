using Gyldendal.Porter.Application.Contracts.Enums;

namespace Gyldendal.Porter.Application.Contracts.Request
{
    public class GetSeriesRequest
    {
        public WebShop WebShop { get; set; }
        public int SeriesId { get; set; }
    }
}

