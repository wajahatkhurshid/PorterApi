using System.Collections.Generic;
using System.Threading.Tasks;
using Gyldendal.Porter.Domain.Contracts.Entities;

namespace Gyldendal.Porter.Domain.Contracts.Repositories
{
    public interface IWorkRepository
    {
        Task<string> UpsertWorkAsync(Work work);
        Task<List<Work>> GetWorksAsync();
        Task<bool> DeleteWorkAsync(string id);
        Task<Work> GetWorkByIdAsync(string id);

        Task<Work> GetWorkByContainerInstanceIdAsync(int containerInstanceId);
        Task<Work> GetByProductIdAsync(int productId);
        Task<Work> GetWorkByWorkReviewIdAsync(int workReviewId);
    }
}
