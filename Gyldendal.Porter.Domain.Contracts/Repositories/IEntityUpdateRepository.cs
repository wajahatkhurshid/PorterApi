using System.Threading.Tasks;
using Gyldendal.Porter.Domain.Contracts.Entities.Queue;

namespace Gyldendal.Porter.Domain.Contracts.Repositories
{
    public interface IEntityUpdateRepository : IDataAccessRepository<EntityUpdate>
    {
        Task<bool> DeleteEntityUpdateAsync(EntityUpdate pe);
    }
}