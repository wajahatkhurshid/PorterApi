using System.Collections.Generic;
using System.Linq;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using System.Threading.Tasks;
using Gyldendal.Porter.Domain.Contracts.Entities;
using MongoDB.Driver;

namespace Gyldendal.Porter.Infrastructure.Repository
{
    public class WorkRepository : BaseRepository<Work>, IWorkRepository
    {
        public WorkRepository(PorterContext context) : base(context)
        {
        }

        public async Task<bool> DeleteWorkAsync(string id)
        {
            return await DeleteAsync(new Work() { Id = id });
        }

        public async Task<List<Work>> GetWorksAsync()
        {
            return await GetAllAsync();
        }

        public async Task<Work> GetWorkByIdAsync(string id)
        {
            return await GetByIdAsync(id);
        }

        public async Task<Work> GetWorkByContainerInstanceIdAsync(int containerInstanceId)
        {
            var workSearch = await Collection.FindAsync(w => w.ContainerInstanceId == containerInstanceId);
            return await workSearch.FirstOrDefaultAsync();
        }

        public async Task<Work> GetByProductIdAsync(int productId)
        {
            var workSearch = await Collection.FindAsync(w => w.ProductIds.Contains(productId));
            return await workSearch.FirstOrDefaultAsync();
        }

        public async Task<Work> GetWorkByWorkReviewIdAsync(int workReviewId)
        {
            var workSearch = await Collection.FindAsync(w => w.WorkReviews.Any(wr => wr.ContainerInstanceId == workReviewId));
            return await workSearch.FirstOrDefaultAsync();
        }

        public async Task<string> UpsertWorkAsync(Work work)
        {
            return await UpsertAsync(work);
        }
    }
}
