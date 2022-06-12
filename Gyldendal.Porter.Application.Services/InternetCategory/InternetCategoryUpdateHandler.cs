using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gyldendal.Porter.Common.Enums;
using Gyldendal.Porter.Domain.Contracts.Entities.Taxonomy;
using Gyldendal.Porter.Domain.Contracts.Enums;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using MediatR;

namespace Gyldendal.Porter.Application.Services.InternetCategory
{
    public class InternetCategoryUpdateHandler : IRequestHandler<InternetCategoryUpdateCommand, bool>
    {
        private readonly ITaxonomyRepository _taxonomyRepository;
        private readonly IInternetCategoryRepository _internetCategoryRepository;

        public InternetCategoryUpdateHandler(ITaxonomyRepository taxonomyRepository, IInternetCategoryRepository internetCategoryRepository)
        {
            _taxonomyRepository = taxonomyRepository;
            _internetCategoryRepository = internetCategoryRepository;
        }

        public async Task<bool> Handle(InternetCategoryUpdateCommand request, CancellationToken cancellationToken)
        {
            var taxonomy = await _taxonomyRepository.GetTaxonomyByIdAsync((int)TaxonomyEnum.InternetCategories);

            var internetCategories = taxonomy.TaxonomyNodes.Select(taxonomyNode => GetInternetCategory(taxonomyNode, taxonomy.TaxonomyNodes)).ToList();

            var updateTasks = internetCategories.Select(internetCategory => Task.Run(() => _internetCategoryRepository.UpsertInternetCategoryAsync(internetCategory), cancellationToken));
            await Task.WhenAll(updateTasks);

            return true;
        }

        private static Domain.Contracts.Entities.MasterData.InternetCategory GetInternetCategory(TaxonomyNode node, IEnumerable<TaxonomyNode> nodes)
        {
            var internetCategory = new Domain.Contracts.Entities.MasterData.InternetCategory
            {
                Id = node.NodeId.ToString(),
                Name = node.Name,
                Level = node.Level,
                Type = GetInternetCategoryTypeByLevel(node.Level)
            };

            if (node.ParentNodeId is null)
            {
                return internetCategory;
            }

            var taxonomyNodes = nodes.ToList();
            var parentNode = taxonomyNodes.FirstOrDefault(x => x.NodeId == node.ParentNodeId);

            internetCategory.Parent = GetInternetCategory(parentNode, taxonomyNodes);

            return internetCategory;
        }

        private static InternetCategoryTypes GetInternetCategoryTypeByLevel(int level)
        {
            switch (level)
            {
                case 0:
                    return InternetCategoryTypes.Shop;
                case 1:
                    return InternetCategoryTypes.Area;
                case 2:
                    return InternetCategoryTypes.Subject;
                case 3:
                    return InternetCategoryTypes.SubArea;
                default:
                    return InternetCategoryTypes.Unknown;
            }
        }
    }
}
