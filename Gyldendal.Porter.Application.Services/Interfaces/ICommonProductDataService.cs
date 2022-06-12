using System.Collections.Generic;
using System.Threading.Tasks;
using Gyldendal.Porter.Domain.Contracts.Entities;
using Gyldendal.Porter.Domain.Contracts.ValueObjects;

namespace Gyldendal.Porter.Application.Services.Interfaces
{
    public interface ICommonProductDataService
    {
        Task<List<string>> GetCommonContributorProductIsbnsAsync(int contributorId);

        Task<List<string>> GetCommonContributorEanProductIsbnsAsync(int contributorId);

        Task<List<string>> GetCommonSeriesProductIsbnsAsync(int seriesId);

        Task<List<string>> GetCommonSeriesEanProductIsbnsAsync(int seriesId);

        Task<List<ContributorProduct>> GetCommonContributorProductsAsync(int contributorId);
        
        Task<List<SeriesProduct>> GetCommonSeriesProductsAsync(int seriesId);
    }
}
