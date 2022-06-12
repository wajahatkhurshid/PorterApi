using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gyldendal.Porter.Application.Contracts;
using Gyldendal.Porter.Common;
using Gyldendal.Porter.Common.Exceptions;
using Gyldendal.Porter.Domain.Contracts.Entities.Taxonomy;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using Gyldendal.Porter.Infrastructure.ExternalClients.Interfaces;

namespace Gyldendal.Porter.Infrastructure.Repository.Taxonomy
{
    /// <summary>
    /// Retrieves taxonomy data from the GPM REST API
    /// </summary>
    public class TaxonomyRepository : ITaxonomyRepository
    {
        private readonly IGpmApiClient _apiClient;

        public TaxonomyRepository(IGpmApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        /// <summary>
        /// Fetches data for a given taxonomy
        /// </summary>
        /// <param name="taxonomyId">ID of the taxonomy</param>
        /// <exception cref="TaxonomyException">If fetches taxonomy is null or empty</exception>
        /// <returns>A taxonomy with a collection of its values</returns>
        public async Task<Domain.Contracts.Entities.Taxonomy.Taxonomy> GetTaxonomyByIdAsync(int taxonomyId)
        {
            var result = await _apiClient.GetTaxonomyAsync(taxonomyId, null, null, new CancellationToken(false));
            var response = new Domain.Contracts.Entities.Taxonomy.Taxonomy();
            response.Id = result.TaxonomyId;
            response.RootNodeIds = result.RootNodeIds.ToList();
            response.TaxonomyNodes = result.Nodes.Select(x => new TaxonomyNode()
            {
                ChildNodeIds = x.ChildrenIds.ToList(),
                Level = x.Level,
                Name = x.Name,
                NodeId = x.NodeId,
                ParentNodeId = x.ParentNodeId
            }).ToList();

            if (response == null || response.TaxonomyNodes.Count == 0)
            {
                throw new TaxonomyException((ulong)ErrorCodes.GetTaxonomyFailure, $"{string.Format(ErrorCodes.GetTaxonomyFailure.GetDescription(), taxonomyId)}");
            }

            return response;
        }
    }
}
