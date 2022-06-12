using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using System.Linq.Expressions;
using Gyldendal.Porter.Application.Contracts.Enums;
using Gyldendal.Porter.Common.Utilities;
using Gyldendal.Porter.Domain.Contracts.Entities.MasterData;
using Gyldendal.Porter.Domain.Contracts.Enums;

namespace Gyldendal.Porter.Infrastructure.Repository
{
    public class InternetCategoryRepository : BaseRepository<InternetCategory>, IInternetCategoryRepository
    {
        public InternetCategoryRepository(PorterContext context) : base(context)
        {
        }

        public async Task<string> UpsertInternetCategoryAsync(InternetCategory internetCategory)
        {
            return await UpsertAsync(internetCategory);
        }

        public async Task<List<InternetCategory>> GetAreasAsync(WebShop webShop)
        {
            var displayNameWebshop = GetDisplayName(webShop) != null ? GetDisplayName(webShop) : webShop.ToString();

            return await SearchForAsync(
                     x => x.Level == 1 &&
                    (x.Parent.Name.ToLower() == displayNameWebshop.ToLower() &&
                     x.Parent.Level == 0));
        }

        public async Task<List<InternetCategory>> GetSubjectsAsync(WebShop webShop, int? areaId)
        {

            var displayNameWebshop = GetDisplayName(webShop) != null ? GetDisplayName(webShop) : webShop.ToString();

            Expression<Func<InternetCategory, bool>> mainPredicate;

            Expression<Func<InternetCategory, bool>> subjectFilter = x => x.Type == InternetCategoryTypes.Subject;

            Expression<Func<InternetCategory, bool>> areaFilter = x => (x.Parent.Id == areaId.ToString() &&
                                                                        x.Parent.Type == InternetCategoryTypes.Area);

            Expression<Func<InternetCategory, bool>> webshopFilter = x => (x.Parent.Parent.Name.ToLower() == displayNameWebshop.ToLower() &&
                                                                           x.Parent.Parent.Type == InternetCategoryTypes.Shop);

            if (areaId.HasValue)
                mainPredicate = subjectFilter.AndAlso(areaFilter).AndAlso(webshopFilter);
            else
                mainPredicate = subjectFilter.AndAlso(webshopFilter);
           
            var internetCategories = await SearchForAsync(mainPredicate);
            return internetCategories;

        }

        public async Task<List<InternetCategory>> GetSubAreasAsync(WebShop webShop, int? subjectId)
        {

            var displayNameWebshop = GetDisplayName(webShop) != null ? GetDisplayName(webShop) : webShop.ToString();

            Expression<Func<InternetCategory, bool>> mainPredicate;
            Expression<Func<InternetCategory, bool>> subAreaFilter = x => x.Type == InternetCategoryTypes.SubArea;
            Expression<Func<InternetCategory, bool>> subjectFilter = x => (x.Parent.Id == subjectId.ToString() &&
                                                                           x.Parent.Type == InternetCategoryTypes.Subject);
            Expression<Func<InternetCategory, bool>> webshopFilter = x => (x.Parent.Parent.Parent.Name.ToLower() == displayNameWebshop.ToLower() &&
                                                                           x.Parent.Parent.Parent.Type == InternetCategoryTypes.Shop);

            if (subjectId.HasValue)
                mainPredicate = subAreaFilter.AndAlso(subjectFilter).AndAlso(webshopFilter);
            else
                mainPredicate = subAreaFilter.AndAlso(webshopFilter);
           
            var internetCategories = await SearchForAsync(mainPredicate);
            return internetCategories;

        }

        private string GetDisplayName(Enum enumValue)
        {
            return enumValue.GetType()?
                .GetMember(enumValue.ToString())?
                .First()?
                .GetCustomAttribute<DisplayAttribute>()?
                .Name;
        }

        public async Task<List<InternetCategory>> GetInternetCategoriesAsync()
        {
            return await GetAllAsync();
        }
    }
}
