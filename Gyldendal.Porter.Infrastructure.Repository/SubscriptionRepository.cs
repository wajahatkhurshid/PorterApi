using Gyldendal.Porter.Domain.Contracts.Entities.Subscription;
using Gyldendal.Porter.Domain.Contracts.Repositories;

namespace Gyldendal.Porter.Infrastructure.Repository
{
    public class SubscriptionRepository : BaseRepository<Subscription>, ISubscriptionRepository
    {
        public SubscriptionRepository(PorterContext context) : base(context)
        {
        }
    }
}
