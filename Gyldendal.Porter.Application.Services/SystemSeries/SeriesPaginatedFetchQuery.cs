using Gyldendal.Porter.Application.Contracts.Enums;
using Gyldendal.Porter.Application.Contracts.Request;
using Gyldendal.Porter.Application.Contracts.Response;
using MediatR;

namespace Gyldendal.Porter.Application.Services.SystemSeries
{
    public class SeriesPaginatedFetchQuery : IRequest<GetSeriesPaginatedResponse>
    {
        public WebShop WebShop { get; set; }
        public SeriesType SeriesType { get; set; }
        public string Subject { get; set; }
        public string Area { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public SortBy SortBy { get; set; }
        public SeriesOrderBy OrderBy { get; set; }
        public SeriesPaginatedFetchQuery(GetSeriesPaginatedRequest request)
        {
            WebShop = request.WebShop;
            SeriesType = request.SeriesType;
            Subject = request.Subject;
            Area = request.Area;
            PageSize = request.PageSize;
            PageIndex = request.PageIndex;
            SortBy = request.SortBy;
            OrderBy = request.OrderBy;
        }

    }
}