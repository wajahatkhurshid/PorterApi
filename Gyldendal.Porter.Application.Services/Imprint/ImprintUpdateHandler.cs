using Gyldendal.Porter.Common.Enums;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gyldendal.Porter.Domain.Contracts.Entities.Taxonomy;

namespace Gyldendal.Porter.Application.Services.Imprint
{
    public class ImprintUpdateHandler : IRequestHandler<ImprintUpdateCommand, bool>
    {
        private readonly IImprintRepository _imprintRepository;
        private readonly ITaxonomyRepository _taxonomyRepository;

        public ImprintUpdateHandler(ITaxonomyRepository taxonomyRepository, IImprintRepository imprintRepository)
        {
            _taxonomyRepository = taxonomyRepository;
            _imprintRepository = imprintRepository;
        }

        public async Task<bool> Handle(ImprintUpdateCommand request, CancellationToken cancellationToken)
        {
            var taxonomy = await _taxonomyRepository.GetTaxonomyByIdAsync((int)TaxonomyEnum.Imprint);
            var imprints = taxonomy.TaxonomyNodes.Select(GetImprint).ToList();

            var updateTasks = imprints
                .Select(mt =>
                    Task.Run(() => _imprintRepository.UpsertImprintAsync(mt), cancellationToken));
            await Task.WhenAll(updateTasks);

            return true;
        }

        #region Private Methods

        private Domain.Contracts.Entities.Imprint GetImprint(TaxonomyNode taxonomyNode)
        {
           return new Domain.Contracts.Entities.Imprint()
           {
               Id = taxonomyNode.NodeId.ToString(),
               Name = taxonomyNode.Name,
               Level = taxonomyNode.Level
           };

        }

        #endregion
    }
}
