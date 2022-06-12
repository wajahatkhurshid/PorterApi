using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gyldendal.Porter.Application.Contracts.Enums;
using Gyldendal.Porter.Application.Services.Interfaces;
using Gyldendal.Porter.Common;
using Gyldendal.Porter.Domain.Contracts.Entities.MasterData;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using Gyldendal.Porter.Domain.Contracts.ValueObjects.Containers;

namespace Gyldendal.Porter.Application.Services
{
    public class EducationSubjectLevelsExtractorService : IEducationSubjectLevelsExtractorService
    {
        private readonly IEducationSubjectLevelRepository _educationSubjectLevelRepository;
        private readonly ILogger _logger;

        public EducationSubjectLevelsExtractorService(IEducationSubjectLevelRepository educationSubjectLevelRepository, ILogger logger)
        {
            _educationSubjectLevelRepository = educationSubjectLevelRepository;
            _logger = logger;
        }

        public async Task<List<EducationLevel>> Extract(List<List<GpmNode>> gpmEducationSubjectLevels)
        {
            var dictEducationSubjectLevels = gpmEducationSubjectLevels
                .Where(x => x.Count == 2).Select(x => x[1])
                .Select(esl => esl.Name)
                .Distinct() // GPM is sending duplicate levels that's why picking distinct
                .ToList();

            return await GetEducationLevels(dictEducationSubjectLevels);
        }

        private async Task<List<EducationLevel>> GetEducationLevels(IEnumerable<string> educationLevels)
        {
            var results = new List<EducationLevel>();

            foreach (var educationLevelName in educationLevels)
            {
                var educationSubjectLevels = await _educationSubjectLevelRepository.GetEducationSubjectLevelsAsync(educationLevelName);

                if (educationSubjectLevels == null || educationSubjectLevels.Count == 0)
                {
                    continue;
                }

                if (educationSubjectLevels.Count > 1)
                {
                    _logger.Info($"DATA CORRUPTION: Multiple EducationSubjectLevels found against Id: {educationLevelName} and Name: {educationLevelName}. It should be single.", isGdprSafe: true);
                    continue;
                }

                results.Add(new EducationLevel
                {
                    Id = Convert.ToInt32(educationSubjectLevels[0].Id),
                    Name = educationSubjectLevels[0].Name,
                    WebShop = (WebShop)Enum.Parse(typeof(WebShop), educationSubjectLevels[0].Parent.Webshop),
                    LevelNumber = educationSubjectLevels[0].Level,
                    AreaId = educationSubjectLevels[0].Parent.AreaId
                });
            }

            return results;
        }
    }
}
