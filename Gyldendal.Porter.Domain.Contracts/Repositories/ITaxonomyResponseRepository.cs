using System.Threading.Tasks;
using Gyldendal.Porter.Domain.Contracts.Entities.Taxonomy;

namespace Gyldendal.Porter.Domain.Contracts.Repositories
{
    public interface ITaxonomyResponseRepository
    {
        Task<bool> InsertTaxonomyResponseAsync(TaxonomyResponse response);
        Task<TaxonomyResponse> FindTaxonomyResponseByTaxonomyIdAsync(int taxonomyId);
    }
}
