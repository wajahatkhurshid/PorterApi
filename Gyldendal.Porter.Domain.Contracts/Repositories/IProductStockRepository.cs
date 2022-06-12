using System.Threading.Tasks;
using Gyldendal.Porter.Domain.Contracts.Entities;

namespace Gyldendal.Porter.Domain.Contracts.Repositories
{
    public interface IProductStockRepository
    {
        Task<string> UpsertProductStockAsync(ProductStock productStock);

        Task<ProductStock> GetProductStockByIdAsync(string id);

        Task<ProductStock> GetProductStockByIsbnAsync(string isbn);
    }
}
