using System;
using Gyldendal.Porter.Domain.Contracts.Entities.Cooked;
using System.Threading.Tasks;

namespace Gyldendal.Porter.Domain.Contracts.Repositories
{
    public interface ICookedWorkReviewRepository : IDataAccessRepository<CookedWorkReview>
    {
        Task<bool> IsWorkReviewCollectionExists();
        Task<long> GetCountByTimeRange(DateTime startDateTime, DateTime endDateTime);
    }
}
