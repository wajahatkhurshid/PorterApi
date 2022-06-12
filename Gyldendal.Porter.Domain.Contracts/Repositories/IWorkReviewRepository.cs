using Gyldendal.Porter.Domain.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gyldendal.Porter.Domain.Contracts.Repositories
{
    public interface IWorkReviewRepository
    {
        Task<List<WorkReview>> GetWorkReviewsAsync();

        Task<string> UpsertWorkReviewAsync(WorkReview workReview);

        Task<WorkReview> GetWorkReviewByIdAsync(string id);

        Task<List<WorkReview>> GetWorkReviewsUpdatedInfoAsync(DateTime UpdatedAfter, int pageIndex, int pageSize);

        Task<int> GetUpdatedWorkReviewsCount(DateTime UpdatedAfter);
    }
}
