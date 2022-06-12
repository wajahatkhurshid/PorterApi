using System.Threading.Tasks;
using Gyldendal.Porter.Domain.Contracts.Entities.Taxonomy;

namespace Gyldendal.Porter.Domain.Contracts.Repositories
{
    public interface ITaxonomyRepository
    {
        Task<Taxonomy> GetTaxonomyByIdAsync(int taxonomyId);
    }
}
