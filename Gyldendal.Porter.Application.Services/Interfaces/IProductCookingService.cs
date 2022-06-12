using System.Threading;
using System.Threading.Tasks;
using Gyldendal.Porter.Application.Services.EntityProcessing;

namespace Gyldendal.Porter.Application.Services.Interfaces
{
    [DisableMultipleQueuedItemsFilter]
    [PreserveCookingQueue("cooking")]
    public interface IProductCookingService
    {
        Task Cook(string isbn, CancellationToken cancellationToken);
    }
}
