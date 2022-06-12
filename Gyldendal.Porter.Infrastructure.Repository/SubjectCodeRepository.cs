using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Gyldendal.Porter.Common;
using Gyldendal.Porter.Domain.Contracts.Entities.MasterData;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace Gyldendal.Porter.Infrastructure.Repository
{
    [BsonCollection("SubjectCode")]
    public class SubjectCodeRepository : BaseRepository<SubjectCode>, ISubjectCodeRepository
    {
        private readonly IMemoryCache _cache;

        public SubjectCodeRepository(PorterContext context, IMemoryCache cache) : base(context)
        {
            _cache = cache;
        }

        public async Task<List<SubjectCode>> GetSubjectCodesAsync()
        {
            return await GetAllAsync();
        }

        public async Task<string> UpsertSubjectCodeAsync(SubjectCode subjectCode)
        {
            return await UpsertAsync(subjectCode);
        }

        public async Task<SubjectCode> GetSubjectCodeAsync(string name)
        {
            Expression<Func<SubjectCode, bool>> filterPredicate = x => x.Name.ToLower() == name.ToLower();
            var cacheKey = $"subject-code-{name.ToLower()}";
            if (_cache.TryGetValue<SubjectCode>(cacheKey, out var cachedValue))
                return cachedValue;

            var subjectCodes = await SearchForAsync(filterPredicate);
            var subjectCode = subjectCodes.FirstOrDefault();

            if (subjectCode != null)
                _cache.Set(cacheKey, subjectCode, TimeSpan.FromHours(3));

            return subjectCode;
        }
    }
}