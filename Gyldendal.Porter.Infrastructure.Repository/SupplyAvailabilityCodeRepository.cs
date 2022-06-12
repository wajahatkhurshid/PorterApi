using System.Threading.Tasks;
using Gyldendal.Porter.Domain.Contracts.Entities.MasterData;
using Gyldendal.Porter.Domain.Contracts.Repositories;

namespace Gyldendal.Porter.Infrastructure.Repository
{
    public class SupplyAvailabilityCodeRepository : BaseRepository<SupplyAvailabilityCode> , ISupplyAvailabilityCodeRepository
    {
        public SupplyAvailabilityCodeRepository(PorterContext context) : base(context)
        {
        }

        public async Task<string> UpsertSubjectCodeAsync(SupplyAvailabilityCode supplyAvailabilityCode)
        {
            return await UpsertAsync(supplyAvailabilityCode);
        }
    }
}
