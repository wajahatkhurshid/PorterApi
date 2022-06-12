using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gyldendal.Porter.Application.Contracts.Enums;
using Gyldendal.Porter.Application.Contracts.Response;
using Gyldendal.Porter.Common.Request;
using Gyldendal.Porter.Domain.Contracts.Entities.Cooked;

namespace Gyldendal.Porter.Domain.Contracts.Repositories
{
    public interface ICookedContributorRepository : IDataAccessRepository<CookedContributor>
    {
        Task<List<CookedContributor>> SearchContributorAsync(string authorName, string propertiesToInclude);
        
        Task<ContributorResponse<CookedContributor>> ContributorSearchAsync(SearchContributorRequest request);
        
        Task<bool> IsContributorCollectionExists();
        Task<CookedContributor> GetContributorById(string id, WebShop webShop);
        Task<long> GetCountByTimeRange(DateTime startDateTime, DateTime endDateTime);
    }
}
