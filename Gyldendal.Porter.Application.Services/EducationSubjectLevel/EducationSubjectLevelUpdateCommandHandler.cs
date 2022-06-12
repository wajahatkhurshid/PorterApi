using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gyldendal.Porter.Domain.Contracts.Entities.Taxonomy;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using MediatR;

namespace Gyldendal.Porter.Application.Services.EducationSubjectLevel
{
    public class EducationSubjectLevelUpdateCommandHandler : IRequestHandler<EducationSubjectLevelUpdateCommand, bool>
    {
        private readonly ITaxonomyRepository _taxonomyRepository;
        private readonly IEducationSubjectLevelRepository _educationSubjectLevelRepository;

        public EducationSubjectLevelUpdateCommandHandler(ITaxonomyRepository taxonomyRepository, IEducationSubjectLevelRepository educationSubjectLevelRepository)
        {
            _taxonomyRepository = taxonomyRepository;
            _educationSubjectLevelRepository = educationSubjectLevelRepository;
        }

        /// <summary>
        /// Retrieves new set of education subject levels. Removes the ones that no longer exist, then upserts the rest.
        /// </summary>
        public async Task<bool> Handle(EducationSubjectLevelUpdateCommand request, CancellationToken cancellationToken)
        {
            var internetCategoryTaxonomy = await _taxonomyRepository.GetTaxonomyByIdAsync(3);
            var educationSubjectLevelTaxonomy = await _taxonomyRepository.GetTaxonomyByIdAsync(10);

            await DeleteRemovedEducationSubjectLevels(educationSubjectLevelTaxonomy);
            await UpdateEducationSubjectLevelData(cancellationToken, educationSubjectLevelTaxonomy, internetCategoryTaxonomy);

            return true;
        }

        private async Task UpdateEducationSubjectLevelData(CancellationToken cancellationToken,
            Taxonomy educationSubjectLevelTaxonomy, Taxonomy internetCategoryTaxonomy)
        {
            var educationSubjectLevels = new List<Domain.Contracts.Entities.MasterData.EducationSubjectLevel>();

            foreach (var educationSubjectLevelNode in educationSubjectLevelTaxonomy.TaxonomyNodes)
            {
                var matchingArea = GetAreaMatchingEducationSubjectLevel(internetCategoryTaxonomy, educationSubjectLevelNode);
                var areaParentNode = GetWebshopNodeById(matchingArea?.ParentNodeId, internetCategoryTaxonomy);

                var educationSubjectLevel =
                    CreateEducationSubjectLevel(educationSubjectLevelNode, matchingArea, areaParentNode, educationSubjectLevelTaxonomy.TaxonomyNodes, internetCategoryTaxonomy);

                educationSubjectLevels.Add(educationSubjectLevel);
            }

            var updateTasks = educationSubjectLevels.Select(esl =>
                Task.Run(async () => await _educationSubjectLevelRepository.UpsertEducationSubjectLevelAsync(esl),
                    cancellationToken));
            await Task.WhenAll(updateTasks);
        }

        /// <summary>
        /// Removes education subject levels by removing any nodes that no longer exist in the new set of data fetched
        /// </summary>
        private async Task DeleteRemovedEducationSubjectLevels(Taxonomy educationSubjectLevelTaxonomy)
        {
            var existingLevels = await _educationSubjectLevelRepository.GetAsync();
            var existingIds = existingLevels.Select(esl => esl.Id);
            var fetchedIds = educationSubjectLevelTaxonomy.TaxonomyNodes.Select(esl => esl.NodeId.ToString());
            var removedIds = existingIds.Where(id => !fetchedIds.Contains(id));
            var removalTasks = removedIds.Select(id =>
                Task.Run(async () => await _educationSubjectLevelRepository.DeleteSubjectEducationLevelsByIdAsync(id)));
            await Task.WhenAll(removalTasks);
        }

        /// <summary>
        /// Finds any Area node in the internet category nodes that matches the education subject level by name
        /// </summary>
        private static TaxonomyNode GetAreaMatchingEducationSubjectLevel(Taxonomy internetCategoryTaxonomy, TaxonomyNode educationSubjectLevelNode)
        {
            return internetCategoryTaxonomy.TaxonomyNodes
                .FirstOrDefault(t => t.Name.Equals(educationSubjectLevelNode.Name, StringComparison.OrdinalIgnoreCase) && t.Level == 1);
        }

        /// <summary>
        /// Returns the webshop node
        /// </summary>
        private TaxonomyNode GetWebshopNodeById(int? nodeId, Taxonomy internetCategoryTaxonomy)
        {
            return internetCategoryTaxonomy.TaxonomyNodes.FirstOrDefault(t => t.NodeId == nodeId && t.Level == 0);
        }

        private Domain.Contracts.Entities.MasterData.EducationSubjectLevel CreateEducationSubjectLevel(TaxonomyNode educationSubjectLevelNode, TaxonomyNode areaNode, TaxonomyNode webshopNode, IEnumerable<TaxonomyNode> nodes, Taxonomy internetCategoryTaxonomy)
        {
            var educationSubjectLevel = new Domain.Contracts.Entities.MasterData.EducationSubjectLevel
            {
                AreaId = areaNode?.NodeId,
                Id = educationSubjectLevelNode.NodeId.ToString(),
                Name = educationSubjectLevelNode.Name,
                Webshop = webshopNode?.Name,
                Level = educationSubjectLevelNode.Level
            };

            if (educationSubjectLevelNode.ParentNodeId is null)
            {
                return educationSubjectLevel;
            }

            var taxonomyNodes = nodes.ToList();

            var parentNode = taxonomyNodes.FirstOrDefault(x => x.NodeId == educationSubjectLevelNode.ParentNodeId);

            var matchingArea = GetAreaMatchingEducationSubjectLevel(internetCategoryTaxonomy, parentNode);

            var areaParentNode = GetWebshopNodeById(matchingArea?.ParentNodeId, internetCategoryTaxonomy);

            educationSubjectLevel.Parent = CreateEducationSubjectLevel(parentNode, matchingArea, areaParentNode, taxonomyNodes, internetCategoryTaxonomy);

            return educationSubjectLevel;
        }
    }
}
