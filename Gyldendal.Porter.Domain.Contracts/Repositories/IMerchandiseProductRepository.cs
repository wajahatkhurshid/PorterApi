using System.Collections.Generic;
using System.Threading.Tasks;
using Gyldendal.Porter.Domain.Contracts.Entities;

namespace Gyldendal.Porter.Domain.Contracts.Repositories
{
    public interface IMerchandiseProductRepository : IDataAccessRepository<MerchandiseProduct>
    {
        Task<List<MerchandiseProduct>> GetMerchandiseProductBySeriesIdAsync(int seriesId, string propertiesToProject);
        Task<List<MerchandiseProduct>> GetMerchandiseProductsByContributorIdAsync(int contributorId, string propertiesToInclude);
    }
}
