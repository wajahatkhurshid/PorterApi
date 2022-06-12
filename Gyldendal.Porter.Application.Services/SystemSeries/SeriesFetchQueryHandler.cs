using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Gyldendal.Porter.Application.Contracts.Models;
using Gyldendal.Porter.Application.Contracts.Response;
using Gyldendal.Porter.Domain.Contracts.Entities.Cooked;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using MediatR;

namespace Gyldendal.Porter.Application.Services.SystemSeries
{
    public class SeriesFetchQueryHandler : IRequestHandler<SeriesFetchQuery, GetSeriesResponse>
    {
        private readonly IMapper _mapper;
        private readonly ICookedSeriesRepository _cookedSeriesRepository;

        public SeriesFetchQueryHandler(ICookedSeriesRepository cookedSeriesRepository, IMapper mapper)
        {
            _cookedSeriesRepository = cookedSeriesRepository;
            _mapper = mapper;
        }

        public async Task<GetSeriesResponse> Handle(SeriesFetchQuery request, CancellationToken cancellationToken)
        {
            var result = await _cookedSeriesRepository.GetSeriesByIdAsync(request.SeriesId.ToString(), request.WebShop);
            return GetSeries(result);
        }

        private GetSeriesResponse GetSeries(CookedSeries series)
        {
            var seriesResponse = new GetSeriesResponse
            {
                Series = _mapper.Map<Series>(series)
            };
            return seriesResponse;
        }
    }
}