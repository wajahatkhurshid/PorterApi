using System.Collections.Generic;
using System.Threading.Tasks;
using Gyldendal.Porter.Domain.Contracts.Entities.MasterData;
using Gyldendal.Porter.Domain.Contracts.ValueObjects.Containers;

namespace Gyldendal.Porter.Application.Services.Interfaces
{
    public interface IEducationSubjectLevelsExtractorService
    {
        Task<List<EducationLevel>> Extract(List<List<GpmNode>> gpmEducationSubjectLevels);
    }
}
