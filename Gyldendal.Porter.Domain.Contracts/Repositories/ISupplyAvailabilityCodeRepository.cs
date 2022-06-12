using System.Threading.Tasks;
using Gyldendal.Porter.Domain.Contracts.Entities.MasterData;

namespace Gyldendal.Porter.Domain.Contracts.Repositories
{
   public interface ISupplyAvailabilityCodeRepository
    {
        Task<string> UpsertSubjectCodeAsync(SupplyAvailabilityCode supplyAvailabilityCode);
    }
}
