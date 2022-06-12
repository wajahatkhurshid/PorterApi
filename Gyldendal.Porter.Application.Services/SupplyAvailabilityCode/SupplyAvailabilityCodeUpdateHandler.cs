using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gyldendal.Porter.Common.Enums;
using Gyldendal.Porter.Domain.Contracts.Entities.Taxonomy;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using MediatR;

namespace Gyldendal.Porter.Application.Services.SupplyAvailabilityCode
{
   public class SupplyAvailabilityCodeUpdateHandler : IRequestHandler<SupplyAvailabilityCodeUpdateCommand, bool>
    {
        private readonly ISupplyAvailabilityCodeRepository _supplyAvailabilityCodeRepository;
        private readonly ITaxonomyRepository _taxonomyRepository;

        public SupplyAvailabilityCodeUpdateHandler(ITaxonomyRepository taxonomyRepository, ISupplyAvailabilityCodeRepository supplyAvailabilityCodeRepository)
        {
            _taxonomyRepository = taxonomyRepository;
            _supplyAvailabilityCodeRepository = supplyAvailabilityCodeRepository;
        }

        public async Task<bool> Handle(SupplyAvailabilityCodeUpdateCommand request, CancellationToken cancellationToken)
        {
            var taxonomy = await _taxonomyRepository.GetTaxonomyByIdAsync((int)TaxonomyEnum.SupplyAvailabilityCode);
            var mediaMaterialTypes = GetSupplyAvailabilityCode(taxonomy);

            var updateTasks = mediaMaterialTypes
                .Select(mt =>
                    Task.Run(() => _supplyAvailabilityCodeRepository.UpsertSubjectCodeAsync(mt), cancellationToken));
            await Task.WhenAll(updateTasks);

            return true;
        }

        #region Private Methods

        private List<Domain.Contracts.Entities.MasterData.SupplyAvailabilityCode> GetSupplyAvailabilityCode(Taxonomy taxonomy)
        {
            var subjectCode = new List<Domain.Contracts.Entities.MasterData.SupplyAvailabilityCode>();

            foreach (var node in taxonomy.TaxonomyNodes)
            {
                subjectCode.Add(new Domain.Contracts.Entities.MasterData.SupplyAvailabilityCode()
                {
                    Id = node.NodeId.ToString(),
                    Name = node.Name,
                    Level = node.Level

                });
            }

            return subjectCode;
        }

        #endregion
    }
}
