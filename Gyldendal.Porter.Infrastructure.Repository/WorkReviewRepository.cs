using System;
using AutoMapper;
using Gyldendal.Porter.Domain.Contracts.Entities;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gyldendal.Porter.Infrastructure.Repository
{
    public class WorkReviewRepository : BaseRepository<WorkReview>, IWorkReviewRepository
    {
        private readonly IMapper _mapper;

        public WorkReviewRepository(PorterContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }
        public async Task<WorkReview> GetWorkReviewByIdAsync(string id)
        {
            return await GetByIdAsync(id);
        }


        public async Task<List<WorkReview>> GetWorkReviewsAsync()
        {
            return await GetAllAsync();
        }

        public async Task<string> UpsertWorkReviewAsync(WorkReview workReview)
        {
            return await UpsertAsync(workReview);
        }

        public async Task<List<WorkReview>> GetWorkReviewsUpdatedInfoAsync(DateTime updatedAfter, int pageIndex, int pageSize)
        {
            return await GetByPaginationAsync(x => x.UpdatedTimestamp >= updatedAfter, pageSize, pageIndex);
        }

        public async Task<int> GetUpdatedWorkReviewsCount(DateTime updatedAfter)
        {
            var response = await SearchForAsync(x => x.UpdatedTimestamp >= updatedAfter);
            return response.Count;
        }
    }
}
