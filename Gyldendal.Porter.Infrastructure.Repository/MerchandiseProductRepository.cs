using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gyldendal.Porter.Domain.Contracts.Entities;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using Gyldendal.Porter.Infrastructure.Repository.HelperExtensions;
using MongoDB.Driver;

namespace Gyldendal.Porter.Infrastructure.Repository
{
    public class MerchandiseProductRepository : BaseRepository<MerchandiseProduct>, IMerchandiseProductRepository
    {
        public MerchandiseProductRepository(PorterContext context) : base(context)
        {
        }

        public async Task<List<MerchandiseProduct>> GetMerchandiseProductBySeriesIdAsync(int seriesId, string propertiesToProject)
        {
            var option = propertiesToProject.GetProjectionFilter<MerchandiseProduct>();
            option.Sort.Descending(x => x.UpdatedTimestamp);
            var products = await Collection.FindAsync(x => x.MerchandiseCollectionTitle.Any(s => s.ContainerInstanceId == seriesId), option).Result.ToListAsync();
            return products;
        }

        public async Task<List<MerchandiseProduct>> GetMerchandiseProductsByContributorIdAsync(int contributorId, string propertiesToInclude)
        {
            var option = propertiesToInclude.GetProjectionFilter<MerchandiseProduct>();
            option.Sort.Descending(x => x.UpdatedTimestamp);
            var productSearch = await Collection.FindAsync(w => w.MerchandiseContributorAuthor.Any(wr => wr.ContainerInstanceId == contributorId), option);
            return await productSearch.ToListAsync();
        }
    }
}
