using System.Collections.Generic;
using System.Threading.Tasks;
using Gyldendal.Porter.Domain.Contracts.Entities.MasterData;


namespace Gyldendal.Porter.Domain.Contracts.Repositories
{
    public interface ISubjectCodeRepository
    {
        Task<string> UpsertSubjectCodeAsync(SubjectCode subjectCode);

        Task<List<SubjectCode>> GetSubjectCodesAsync();

        Task<SubjectCode> GetSubjectCodeAsync(string name);
    }
}
