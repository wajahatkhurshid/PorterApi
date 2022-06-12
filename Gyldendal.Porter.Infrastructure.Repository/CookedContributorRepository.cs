using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Gyldendal.Porter.Application.Contracts.Enums;
using Gyldendal.Porter.Application.Contracts.Response;
using Gyldendal.Porter.Common.Request;
using Gyldendal.Porter.Domain.Contracts.Entities.Cooked;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using Gyldendal.Porter.Infrastructure.Repository.HelperExtensions;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Gyldendal.Porter.Infrastructure.Repository
{
    public class CookedContributorRepository : BaseRepository<CookedContributor>, ICookedContributorRepository
    {
        public CookedContributorRepository(PorterContext context) : base(context)
        {
        }

        public async Task<List<CookedContributor>> SearchContributorAsync(string authorName, string propertiesToInclude)
        {
            var option = propertiesToInclude.GetProjectionFilter<CookedContributor>();

            var queryCursor = await Collection.FindAsync(x => x.FirstName.Contains(authorName),
                option);

            return await queryCursor.ToListAsync();
        }

        public async Task<ContributorResponse<CookedContributor>> ContributorSearchAsync(SearchContributorRequest request)
        {
            var option = request.PropertiesToInclude.GetProjectionFilter<CookedContributor>();
            var filters = GetSearchFilter(request);
            option.Skip = (request.Page - 1) * request.PageSize;
            option.Limit = request.PageSize;

            option.Collation = new Collation("en", strength: CollationStrength.Secondary);
            var queryCursor = await Collection.FindAsync(filters,
                option);

            var contributor = await queryCursor.ToListAsync();

            var contributorCount = await Collection.CountDocumentsAsync(filters);

            var response = new ContributorResponse<CookedContributor>
            {
                Count = (int)contributorCount,
                Results = contributor
            };
            return response;
        }

        private static FilterDefinition<CookedContributor> GetSearchFilter(SearchContributorRequest request)
        {
            var filter = Builders<CookedContributor>.Filter.Empty;

            if (!string.IsNullOrWhiteSpace(request.SearchString))
            {
                filter &= Builders<CookedContributor>.Filter.Regex(x => x.FirstName,
                    new BsonRegularExpression(new Regex(request.SearchString,
                        RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace)));
            }
            return filter;
        }

        public async Task<bool> IsContributorCollectionExists()
        {
            return await Collection.EstimatedDocumentCountAsync() > 0;
        }

        public async Task<CookedContributor> GetContributorById(string id, WebShop webShop)
        {
            var queryCursor = await Collection.FindAsync(x => x.Id == id && x.WebShops.Contains(webShop));

            return await queryCursor.SingleOrDefaultAsync();
        }

        public async Task<long> GetCountByTimeRange(DateTime startDateTime, DateTime endDateTime)
        {
            var definition = new FilterDefinitionBuilder<CookedContributor>()
                .Where(s => s.UpdatedTimestamp >= startDateTime && s.UpdatedTimestamp <= endDateTime);
            return await Collection.CountDocumentsAsync(definition);
        }
    }
}
