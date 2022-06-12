using System.Threading.Tasks;

namespace Gyldendal.Porter.Domain.Contracts.Interfaces
{
    public interface IProductStockClient
    {
        Task<int> FetchAvailableStockAsync(string isbn);
    }
}
