using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gyldendal.Porter.Domain.Contracts.Entities;
using Gyldendal.Porter.Domain.Contracts.Repositories;

namespace Gyldendal.Porter.Infrastructure.Repository
{
    public class SeriesRepository : BaseRepository<Series>, ISeriesRepository
    {
        public SeriesRepository(PorterContext context) : base(context)
        {
        }

        public async Task<bool> DeleteSeriesAsync(string id)
        {
            return await DeleteAsync(new Series { Id = id });
        }

        public async Task<List<Series>> GetSeriesAsync()
        {
            return await GetAllAsync();
        }

        public async Task<string> UpsertSeriesAsync(Series series)
        {
            return await UpsertAsync(series);
        }

        public async Task<Series> GetSeriesByIdAsync(string id)
        {
            return await GetByIdAsync(id);
        }

        public async Task<List<Series>> GetByIdsAsync(IEnumerable<string> ids)
        {
            return await SearchForAsync(x => ids.Contains(x.Id));
        }
    }
}
