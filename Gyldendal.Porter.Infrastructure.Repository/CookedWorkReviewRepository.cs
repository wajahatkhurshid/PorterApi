using System;
using System.Threading.Tasks;
using Gyldendal.Porter.Domain.Contracts.Entities.Cooked;
using Gyldendal.Porter.Domain.Contracts.Repositories;

namespace Gyldendal.Porter.Infrastructure.Repository
{
    public class CookedWorkReviewRepository : BaseRepository<CookedWorkReview>, ICookedWorkReviewRepository
    {
        public CookedWorkReviewRepository(PorterContext context) : base(context)
        {
        }

        public async Task<bool> IsWorkReviewCollectionExists()
        {
            return await Collection.EstimatedDocumentCountAsync() > 0;
        }

        public async Task<long> GetCountByTimeRange(DateTime startDateTime, DateTime endDateTime)
        {
            var definition = new MongoDB.Driver.FilterDefinitionBuilder<CookedWorkReview>()
                .Where(s => s.UpdatedTimestamp >= startDateTime && s.UpdatedTimestamp <= endDateTime);
            return await Collection.CountDocumentsAsync(definition);
        }
    }
}
