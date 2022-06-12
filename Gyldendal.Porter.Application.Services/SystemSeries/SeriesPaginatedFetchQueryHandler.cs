using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Gyldendal.Porter.Application.Contracts.Models;
using Gyldendal.Porter.Application.Contracts.Response;
using Gyldendal.Porter.Domain.Contracts.Entities.Cooked;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using Gyldendal.Porter.Domain.Contracts.ValueObjects.Taxonomy;
using MediatR;

namespace Gyldendal.Porter.Application.Services.SystemSeries
{
    public class SeriesPaginatedFetchQueryHandler : IRequestHandler<SeriesPaginatedFetchQuery, GetSeriesPaginatedResponse>
    {
        private readonly IMapper _mapper;
        private readonly ICookedSeriesRepository _cookedSeriesRepository;

        public SeriesPaginatedFetchQueryHandler(ICookedSeriesRepository cookedSeriesRepository, IMapper mapper)
        {
            _cookedSeriesRepository = cookedSeriesRepository;
            _mapper = mapper;
        }

        public async Task<GetSeriesPaginatedResponse> Handle(SeriesPaginatedFetchQuery request, CancellationToken cancellationToken)
        {
            var sortAndPaginate = new SortAndPaginate
            {
                OrderBy = request.OrderBy.ToString(),
                SortBy = request.SortBy,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            var result = await _cookedSeriesRepository.GetSeriesPaginationByAsync(request.WebShop, request.SeriesType, sortAndPaginate, request.Area, request.Subject);

            var count = await _cookedSeriesRepository.GetCountAsync(request.WebShop, request.SeriesType, request.Area, request.Subject);
            
            return GetResponse(result, count, request.PageIndex, request.PageSize);
        }

        private GetSeriesPaginatedResponse GetResponse(List<CookedSeries> series, int count,
            int pageIndex, int pageSize)
        {
            var seriesPaginatedResponse = new GetSeriesPaginatedResponse
            {
                Series = _mapper.Map<List<Series>>(series),
                Count = count,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            return seriesPaginatedResponse;
        }
    }
}