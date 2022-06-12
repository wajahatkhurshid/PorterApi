using System.Threading.Tasks;
using Gyldendal.Porter.Domain.Contracts.Entities;

namespace Gyldendal.Porter.Domain.Contracts.Repositories
{
    public interface IImprintRepository
    {
        Task<string> UpsertImprintAsync(Imprint imprint);
    }
}
