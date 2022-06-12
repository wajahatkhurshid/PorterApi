using System.Collections.Generic;
using System.Threading.Tasks;
using Gyldendal.Porter.Domain.Contracts.Entities.Cooked;
using Gyldendal.Porter.Domain.Contracts.ValueObjects.Containers;

namespace Gyldendal.Porter.Application.Services.Interfaces
{
    public interface IInternetSubjectCookerService
    {
        Task<CookedInternetSubjectWithCode> Cook(List<List<GpmNode>> internetSubjects, List<List<GpmNode>> subjectCodesTaxonomy);
    }
}
