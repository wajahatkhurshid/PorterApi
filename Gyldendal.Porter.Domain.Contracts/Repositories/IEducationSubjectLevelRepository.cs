using System.Collections.Generic;
using System.Threading.Tasks;
using Gyldendal.Porter.Application.Contracts.Enums;
using Gyldendal.Porter.Domain.Contracts.Entities.MasterData;

namespace Gyldendal.Porter.Domain.Contracts.Repositories
{
    public interface IEducationSubjectLevelRepository
    {
        Task<string> UpsertEducationSubjectLevelAsync(EducationSubjectLevel educationSubjectLevel);
        
        Task DeleteSubjectEducationLevelsByIdAsync(string id);
        
        Task<List<EducationSubjectLevel>> GetAsync();
        
        Task<List<EducationSubjectLevel>> GetLevelsAsync(WebShop webShop, int? areaId);

        Task<List<EducationSubjectLevel>> GetEducationSubjectLevelsAsync(string name);
    }
}
