using System.Threading;
using System.Threading.Tasks;
using Gyldendal.Porter.Application.Services.EntityProcessing;

namespace Gyldendal.Porter.Application.Services.Interfaces
{
    [DisableMultipleQueuedItemsFilter]
    [PreserveCookingQueue("cooking")]
    public interface IWorkReviewCookingService
    {
        Task Cook(int workReviewId, CancellationToken cancellationToken);
    }
}
