using Gyldendal.Porter.Application.Contracts.Enums;
using Gyldendal.Porter.Application.Contracts.Response;
using MediatR;

namespace Gyldendal.Porter.Application.Services.SystemSeries
{
    public class SeriesFetchQuery : IRequest<GetSeriesResponse>
    {
        public WebShop WebShop { get; set; }

        public int SeriesId { get; set; }

        public SeriesFetchQuery(WebShop webShop, int seriesId)
        {
            WebShop = webShop;
            SeriesId = seriesId;
        }
    }
}