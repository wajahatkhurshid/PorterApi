using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using Gyldendal.Porter.Common.Enums;
using Gyldendal.Porter.Domain.Contracts.Entities.Taxonomy;
using MediatR;

namespace Gyldendal.Porter.Application.Services.MediaMaterialType
{
   public class MediaMaterialTypeUpdateHandler : IRequestHandler<MediaMaterialTypeUpdateCommand,bool>
    {
       private readonly IMediaMaterialTypeRepository _mediaMaterialTypeRepository;
       private readonly ITaxonomyRepository _taxonomyRepository;

       public MediaMaterialTypeUpdateHandler(IMediaMaterialTypeRepository mediaMaterialTypeRepository, ITaxonomyRepository taxonomyRepository)
       {
           _mediaMaterialTypeRepository = mediaMaterialTypeRepository;
           _taxonomyRepository = taxonomyRepository;
       }

        public async Task<bool> Handle(MediaMaterialTypeUpdateCommand request, CancellationToken cancellationToken)
        {
            var taxonomy = await _taxonomyRepository.GetTaxonomyByIdAsync((int)TaxonomyEnum.MediaMaterialType);
            var mediaMaterialTypes = GetMediaMaterialType(taxonomy);

            var updateTasks = mediaMaterialTypes
                .Select(mt => 
                    Task.Run(() => _mediaMaterialTypeRepository.UpsertMediaMaterialTypeAsync(mt), cancellationToken));
            await Task.WhenAll(updateTasks);

            return true;
        }

        #region Private Methods

        private List<Domain.Contracts.Entities.MasterData.MediaMaterialType> GetMediaMaterialType(Taxonomy taxonomy)
        {
            var mediaMaterialTypes = new List<Domain.Contracts.Entities.MasterData.MediaMaterialType>();

            foreach (var node in taxonomy.TaxonomyNodes)
            {
                if (node.Level == 0)
                {
                    mediaMaterialTypes.Add(GetMediaTypeNode(node));
                }
                else
                {
                    var parent = taxonomy.TaxonomyNodes.FirstOrDefault(x => x.NodeId == node.ParentNodeId);
                    mediaMaterialTypes.Add(GetMaterialTypeNode(node, parent));
                }
            }

            return mediaMaterialTypes;
        }

        private Domain.Contracts.Entities.MasterData.MediaMaterialType GetMediaTypeNode(TaxonomyNode node)
        {
            return new Domain.Contracts.Entities.MasterData.MediaMaterialType
            {
                Id = node.NodeId.ToString(),
                Name = node.Name,
                Level = node.Level
            };
        }

        private Domain.Contracts.Entities.MasterData.MediaMaterialType GetMaterialTypeNode(TaxonomyNode node, TaxonomyNode parent)
        {
            return new Domain.Contracts.Entities.MasterData.MediaMaterialType
            {
                Id = node.NodeId.ToString(),
                Name = node.Name,
                Level = node.Level,
                Parent = GetMediaTypeNode(parent)
            };
        }

        #endregion
    }
}
