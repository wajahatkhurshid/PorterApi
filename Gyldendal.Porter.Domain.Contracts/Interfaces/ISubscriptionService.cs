using System.Collections.Generic;
using System.Threading.Tasks;
using Gyldendal.Porter.Common.Response;
using Gyldendal.Porter.Domain.Contracts.Entities.Subscription;

namespace Gyldendal.Porter.Domain.Contracts.Interfaces
{
    public interface ISubscriptionService
    {
        Task<GpmSubscriptionResponse> Create();

        Task<List<Subscription>> GetSubscriptions();
    }
}
