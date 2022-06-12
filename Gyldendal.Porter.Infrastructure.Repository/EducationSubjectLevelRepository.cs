using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Gyldendal.Porter.Application.Contracts.Enums;
using Gyldendal.Porter.Common.Utilities;
using Gyldendal.Porter.Domain.Contracts.Entities.MasterData;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace Gyldendal.Porter.Infrastructure.Repository
{
    public class EducationSubjectLevelRepository : BaseRepository<EducationSubjectLevel>, IEducationSubjectLevelRepository
    {
        private readonly IMemoryCache _cache;

        public EducationSubjectLevelRepository(PorterContext context, IMemoryCache cache) : base(context)
        {
            _cache = cache;
        }

        public async Task<string> UpsertEducationSubjectLevelAsync(EducationSubjectLevel educationSubjectLevel)
        {
            return await UpsertAsync(educationSubjectLevel);
        }

        public async Task DeleteSubjectEducationLevelsByIdAsync(string id)
        {
            await DeleteAsync(new EducationSubjectLevel {Id = id});
        }

        public async Task<List<EducationSubjectLevel>> GetAsync()
        {
            return await GetAllAsync();
        }

        public async Task<List<EducationSubjectLevel>> GetLevelsAsync(WebShop webShop, int? areaId)
        {
            var displayNameWebshop = GetDisplayName(webShop) != null ? GetDisplayName(webShop) : webShop.ToString();

            Expression<Func<EducationSubjectLevel, bool>> mainPredicate =  x => x.Level == 1 &&
                                                                                     x.Parent.Webshop.ToLower() == displayNameWebshop.ToLower() && 
                                                                                     x.Parent.Level == 0;

            Expression<Func<EducationSubjectLevel, bool>> areaFilter = x => x.Parent.AreaId == areaId;

            if (areaId.HasValue)
                mainPredicate = mainPredicate.AndAlso(areaFilter);

            var educationSubjectLevels = await SearchForAsync(mainPredicate);
            return educationSubjectLevels;
        }

        public async Task<List<EducationSubjectLevel>> GetEducationSubjectLevelsAsync(string name)
        {
            var cacheKey = "education-subject-levels";
            if (_cache.TryGetValue<List<EducationSubjectLevel>>(cacheKey, out var cachedValues)) return cachedValues;

            Expression<Func<EducationSubjectLevel, bool>> filterPredicate = x => x.Level == 1
                && x.Name.ToLower() == name.ToLower();

            var educationSubjectLevels = await SearchForAsync(filterPredicate);
                
            if (educationSubjectLevels != null)
                _cache.Set(cacheKey, educationSubjectLevels, TimeSpan.FromHours(3));

            return educationSubjectLevels;
        }

        private string GetDisplayName(Enum enumValue)
        {
            return enumValue.GetType()?
                .GetMember(enumValue.ToString())?
                .First()?
                .GetCustomAttribute<DisplayAttribute>()?
                .Name;
        }
    }
}
