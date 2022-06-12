using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Gyldendal.Porter.Domain.Contracts.Entities.Taxonomy;
using Gyldendal.Porter.Domain.Contracts.Repositories;

namespace Gyldendal.Porter.Infrastructure.Repository.Taxonomy
{
    public class TaxonomyResponseRepository : BaseRepository<TaxonomyResponse>, ITaxonomyResponseRepository
    {
        private readonly IMapper _mapper;

        public TaxonomyResponseRepository(PorterContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public async Task<bool> InsertTaxonomyResponseAsync(TaxonomyResponse response)
        {
            response.Id = Guid.NewGuid().ToString();
            response.Created = DateTime.UtcNow;
            return await InsertAsync(response);
        }

        public async Task<TaxonomyResponse> FindTaxonomyResponseByTaxonomyIdAsync(int taxonomyId)
        {
            var searchResult = await SearchForAsync(x => x.TaxonomyId == taxonomyId);
            var firstSearchResult = searchResult.FirstOrDefault();
            return firstSearchResult;
        }
    }
}