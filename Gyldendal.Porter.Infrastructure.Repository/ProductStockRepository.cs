using System.Threading.Tasks;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using System.Linq;
using Gyldendal.Porter.Domain.Contracts.Entities;

namespace Gyldendal.Porter.Infrastructure.Repository
{
    public class ProductStockRepository : BaseRepository<ProductStock>, IProductStockRepository
    {
        public ProductStockRepository(PorterContext context) : base(context)
        {
        }

        public async Task<string> UpsertProductStockAsync(ProductStock productStock)
        {
            return await UpsertAsync(productStock);
        }

        public async Task<ProductStock> GetProductStockByIdAsync(string id)
        {
            return await GetByIdAsync(id);
        }

        public async Task<ProductStock> GetProductStockByIsbnAsync(string isbn)
        {
            var response = await SearchForAsync(x => x.Isbn == isbn);
            return response.FirstOrDefault();
        }
    }
}
