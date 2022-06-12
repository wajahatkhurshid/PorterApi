using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gyldendal.Porter.Common.Enums;
using Gyldendal.Porter.Domain.Contracts.Entities.Taxonomy;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using MediatR;
namespace Gyldendal.Porter.Application.Services.SubjectCode
{
    public class SubjectCodeUpdateHandler : IRequestHandler<SubjectCodeUpdateCommand, bool>
    {
        private readonly ISubjectCodeRepository _subjectCodeRepository;
        private readonly ITaxonomyRepository _taxonomyRepository;

        public SubjectCodeUpdateHandler(ITaxonomyRepository taxonomyRepository, ISubjectCodeRepository subjectCodeRepository)
        {
            _taxonomyRepository = taxonomyRepository;
            _subjectCodeRepository = subjectCodeRepository;
        }

        public async Task<bool> Handle(SubjectCodeUpdateCommand request, CancellationToken cancellationToken)
        {
            var taxonomy = await _taxonomyRepository.GetTaxonomyByIdAsync((int)TaxonomyEnum.SubjectCodes);
            var subjectCodes = GetSubjectCodes(taxonomy);

            foreach (var subjectCode in subjectCodes)
            {
                await _subjectCodeRepository.UpsertSubjectCodeAsync(subjectCode);
            }
            
            return true;
        }

        #region Private Methods

        private List<Domain.Contracts.Entities.MasterData.SubjectCode> GetSubjectCodes(Taxonomy taxonomy)
        {
            var subjectCode = new List<Domain.Contracts.Entities.MasterData.SubjectCode>();

            foreach (var node in taxonomy.TaxonomyNodes)
            {
                subjectCode.Add(GetSubjectCodeLevel(node, taxonomy));
            }

            return subjectCode;
        }

        private Domain.Contracts.Entities.MasterData.SubjectCode GetSubjectCodeLevel(TaxonomyNode node, Taxonomy taxonomy)
        {
            if (node.Level == 0)
            {
                return new Domain.Contracts.Entities.MasterData.SubjectCode
                {
                    Id = node.NodeId.ToString(),
                    Name = node.Name,
                    Level = node.Level
                };
            }

            return new Domain.Contracts.Entities.MasterData.SubjectCode
            {
                Id = node.NodeId.ToString(),
                Level = node.Level,
                Name = node.Name,
                Parent = GetSubjectCodeLevel(taxonomy.TaxonomyNodes.First(x => x.NodeId == node.ParentNodeId), taxonomy)
            };

        }
        #endregion
    }
}
