using Gyldendal.Porter.Application.Contracts.Enums;

namespace Gyldendal.Porter.Application.Contracts.Request
{
    public class GetSeriesPaginatedRequest
    {
        public WebShop WebShop { get; set; }
        public SeriesType SeriesType { get; set; }
        public string Subject { get; set; }
        public string Area { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public SortBy SortBy { get; set; }
        public SeriesOrderBy OrderBy { get; set; }
    }
}

