using System.Collections.Generic;
using System.Threading.Tasks;
using Gyldendal.Porter.Domain.Contracts.Entities;

namespace Gyldendal.Porter.Domain.Contracts.Repositories
{
    public interface ISeriesRepository
    {
        Task<string> UpsertSeriesAsync(Series series);
        Task<bool> DeleteSeriesAsync(string id);
        Task<List<Series>> GetSeriesAsync();
        Task<Series> GetSeriesByIdAsync(string id);
        Task<List<Series>> GetByIdsAsync(IEnumerable<string> ids);
    }
}
