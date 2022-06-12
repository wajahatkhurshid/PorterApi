using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Gyldendal.Porter.Application.Contracts.Enums;
using Gyldendal.Porter.Common.Utilities;
using Gyldendal.Porter.Domain.Contracts.Entities.Cooked;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using Gyldendal.Porter.Domain.Contracts.ValueObjects.Taxonomy;

namespace Gyldendal.Porter.Infrastructure.Repository
{
    public class CookedSeriesRepository : BaseRepository<CookedSeries>, ICookedSeriesRepository
    {
        public CookedSeriesRepository(PorterContext context) : base(context)
        {
        }

        public async Task UpsertSeries(CookedSeries series)
        {
            await UpsertAsync(series);
        }

        public async Task<List<CookedSeries>> GetSeries(List<string> seriesIds)
        {
            var series = await SearchForAsync(x => seriesIds.Contains(x.Id));
            return series;
        }

        public async Task<CookedSeries> GetSeriesByIdAsync(string id, WebShop webShop)
        {
            Expression<Func<CookedSeries, bool>> mainPredicate = x => x.WebShops.Contains(webShop);
            Expression<Func<CookedSeries, bool>> idPredicate = x => x.Id == id;
            mainPredicate = mainPredicate.AndAlso(idPredicate);
            
            var response = await SearchForAsync(mainPredicate);
            return response.SingleOrDefault();
        }

        public async Task<int> GetCountAsync(WebShop webShop, SeriesType seriesRequestedType, string area, string subject)
        {
            var predicate = GetExpressionBasedOnFilters(webShop, seriesRequestedType, area, subject);

            var response = await SearchForAsync(predicate);

            return response.Count();
        }

        public async Task<List<CookedSeries>> GetSeriesPaginationByAsync(WebShop webShop, SeriesType seriesRequestedType, SortAndPaginate sortAndPaginate,
            string area, string subject)
        {

            var predicate = GetExpressionBasedOnFilters(webShop, seriesRequestedType, area, subject);

            var response = await GetBySortingAndPaginationAsync(predicate, sortAndPaginate.SortBy.ToString(), sortAndPaginate.OrderBy, sortAndPaginate.PageSize, sortAndPaginate.PageIndex);
            return response;
        }

        private static Expression<Func<CookedSeries, bool>> GetExpressionBasedOnFilters(WebShop webShop, SeriesType seriesType, string area, string subject)
        {
            Expression<Func<CookedSeries, bool>> mainPredicate = x => x.WebShops.Contains(webShop);
            Expression<Func<CookedSeries, bool>> systemSeriesPredicate = x => x.IsSystemSeries == true;
            Expression<Func<CookedSeries, bool>> seriesPredicate = x => x.IsSystemSeries == false;
            Expression<Func<CookedSeries, bool>> areaPredicate = x => x.Areas.Any(a => a.Name.Contains(area));
            Expression<Func<CookedSeries, bool>> subjectPredicate = x => x.Subjects.Any(s => s.Name.Contains(subject));

            if (seriesType == SeriesType.SystemSeries)
                mainPredicate = mainPredicate.AndAlso(systemSeriesPredicate);
            if (seriesType == SeriesType.Series)
                mainPredicate = mainPredicate.AndAlso(seriesPredicate);
            if (string.IsNullOrEmpty(area))
                mainPredicate = mainPredicate.AndAlso(areaPredicate);
            if (string.IsNullOrEmpty(subject))
                mainPredicate = mainPredicate.AndAlso(subjectPredicate);

            return mainPredicate;
        }

        public async Task<long> GetCountByTimeRange(DateTime startDateTime, DateTime endDateTime)
        {
            var definition = new MongoDB.Driver.FilterDefinitionBuilder<CookedSeries>()
                .Where(s => s.UpdatedTimestamp >= startDateTime && s.UpdatedTimestamp <= endDateTime);
            return await Collection.CountDocumentsAsync(definition);
        }
    }
}
