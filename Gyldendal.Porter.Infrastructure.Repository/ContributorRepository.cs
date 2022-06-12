using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Gyldendal.Porter.Application.Contracts.Response;
using Gyldendal.Porter.Common.Request;
using Gyldendal.Porter.Domain.Contracts.Entities;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using Gyldendal.Porter.Infrastructure.Repository.HelperExtensions;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Gyldendal.Porter.Infrastructure.Repository
{
    public class ContributorRepository : BaseRepository<Contributor>, IContributorRepository
    {
        public ContributorRepository(PorterContext context) : base(context)
        {
        }

        public new async Task<string> UpsertAsync(Contributor contributor)
        {
            return await base.UpsertAsync(contributor);
        }

        public async Task<bool> DeleteAllContributorsAsync()
        {
            return await DeleteAllAsync();
        }

        public async Task<bool> DeleteAsync(string id)
        {
            return await DeleteAsync(new Contributor() { Id = id });
        }

        public async Task<List<Contributor>> GetListAsync()
        {
            return await GetAllAsync();
        }

        public async Task<List<Contributor>> GetByIdsAsync(IEnumerable<string> ids)
        {
            var response = await SearchForAsync(x => ids.Contains(x.Id));
            return response;
        }

        public async Task<Contributor> GetContributorByIdAsync(string id)
        {
            return await GetByIdAsync(id);
        }

        public async Task<List<Contributor>> SearchContributorAsync(string authorName, string propertiesToInclude)
        {
            var option = propertiesToInclude.GetProjectionFilter<Contributor>();

            var queryCursor = await Collection.FindAsync(x => x.FirstName.Contains(authorName),
                option);

            return await queryCursor.ToListAsync();
        }

        public async Task<ContributorResponse<Contributor>> ContributorSearchAsync(SearchContributorRequest request)
        {
            var option = request.PropertiesToInclude.GetProjectionFilter<Contributor>();
            var filters = GetSearchFilter(request);
            option.Skip = (request.Page - 1) * request.PageSize;
            option.Limit = request.PageSize;

            option.Collation = new Collation("en", strength: CollationStrength.Secondary);
            var queryCursor = await Collection.FindAsync(filters,
                option);

            var contributor = await queryCursor.ToListAsync();
            
            var contributorCount = await Collection.CountDocumentsAsync(filters);

            var response = new ContributorResponse<Contributor>
            {
                Count = (int)contributorCount,
                Results = contributor
            };
            return response;
        }

        private static FilterDefinition<Contributor> GetSearchFilter(SearchContributorRequest request)
        {
            var filter = Builders<Contributor>.Filter.Empty;

            if (!string.IsNullOrWhiteSpace(request.SearchString))
            {
                filter &= Builders<Contributor>.Filter.Regex(x => x.FirstName,
                    new BsonRegularExpression(new Regex(request.SearchString,
                        RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace)));
            }
            return filter;
        }
    }
}
