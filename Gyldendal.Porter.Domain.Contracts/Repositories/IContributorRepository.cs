using System.Collections.Generic;
using System.Threading.Tasks;
using Gyldendal.Porter.Application.Contracts.Response;
using Gyldendal.Porter.Common.Request;
using Gyldendal.Porter.Domain.Contracts.Entities;

namespace Gyldendal.Porter.Domain.Contracts.Repositories
{
    public interface IContributorRepository
    {
        Task<string> UpsertAsync(Contributor contributor);
        Task<bool> DeleteAsync(string id);
        Task<List<Contributor>> GetListAsync();
        Task<List<Contributor>> GetByIdsAsync(IEnumerable<string> ids);
        Task<Contributor> GetContributorByIdAsync(string id);
        Task<List<Contributor>> SearchContributorAsync(string authorName, string propertiesToInclude);
        Task<ContributorResponse<Contributor>> ContributorSearchAsync(SearchContributorRequest request);
    }
}