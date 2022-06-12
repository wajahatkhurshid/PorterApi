using System.Collections.Generic;
using System.Threading.Tasks;
using Gyldendal.Porter.Application.Contracts.Enums;
using Gyldendal.Porter.Domain.Contracts.Entities.MasterData;

namespace Gyldendal.Porter.Domain.Contracts.Repositories
{
    public interface IInternetCategoryRepository
    {
        Task<string> UpsertInternetCategoryAsync(InternetCategory internetCategory);
        Task<List<InternetCategory>> GetInternetCategoriesAsync();
        Task<List<InternetCategory>> GetAreasAsync(WebShop webShop);
        Task<List<InternetCategory>> GetSubjectsAsync(WebShop webShop, int? areaId);
        Task<List<InternetCategory>> GetSubAreasAsync(WebShop webShop, int? subjectId);

    }
}
