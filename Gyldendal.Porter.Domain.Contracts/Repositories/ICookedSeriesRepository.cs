using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gyldendal.Porter.Application.Contracts.Enums;
using Gyldendal.Porter.Domain.Contracts.Entities.Cooked;
using Gyldendal.Porter.Domain.Contracts.ValueObjects.Taxonomy;

namespace Gyldendal.Porter.Domain.Contracts.Repositories
{
    public interface ICookedSeriesRepository : IDataAccessRepository<CookedSeries>
    {
        Task UpsertSeries(CookedSeries product);
        Task<List<CookedSeries>> GetSeries(List<string> seriesIds);
        Task<CookedSeries> GetSeriesByIdAsync(string id, WebShop webShop);
        Task<int> GetCountAsync(WebShop webShop, SeriesType seriesRequestedType, string area, string subject);
        Task<List<CookedSeries>> GetSeriesPaginationByAsync(WebShop webShop, SeriesType seriesType, SortAndPaginate sortAndPaginate, string area, string subject);
        Task<long> GetCountByTimeRange(DateTime startDateTime, DateTime endDateTime);
    }
}
