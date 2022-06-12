using Gyldendal.Porter.Domain.Contracts.Repositories;
using System.Threading.Tasks;
using Gyldendal.Porter.Domain.Contracts.Entities;

namespace Gyldendal.Porter.Infrastructure.Repository
{
    public class ImprintRepository : BaseRepository<Imprint>, IImprintRepository
    {
        public ImprintRepository(PorterContext context) : base(context)
        {
        }
        
        public async Task<string> UpsertImprintAsync(Imprint imprint)
        {
            return await UpsertAsync(imprint);
        }

    }
}

