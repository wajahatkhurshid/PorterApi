using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Gyldendal.Porter.Application.Contracts.Response;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using MediatR;
using Gyldendal.Porter.Application.Contracts.Enums;
using Gyldendal.Porter.Application.Contracts.Models;

namespace Gyldendal.Porter.Application.Services.EducationSubjectLevel
{
    public class LevelFetchQueryHandler : IRequestHandler<LevelFetchQuery, GetLevelsResponse>
    {
        private readonly IEducationSubjectLevelRepository _educationSubjectLevelRepository;

        public LevelFetchQueryHandler(IEducationSubjectLevelRepository educationSubjectLevelRepository)
        {
            _educationSubjectLevelRepository = educationSubjectLevelRepository;
        }

        public async Task<GetLevelsResponse> Handle(LevelFetchQuery request, CancellationToken cancellationToken)
        {
            var educationalSubjectLevels = await _educationSubjectLevelRepository.GetLevelsAsync(request.LevelsRequest.WebShop, request.LevelsRequest.AreaId);
            return GetLevels(educationalSubjectLevels, request.LevelsRequest.WebShop);
        }

        public GetLevelsResponse GetLevels(List<Domain.Contracts.Entities.MasterData.EducationSubjectLevel> educationSubjectLevels, WebShop webShop)
        {
            GetLevelsResponse levelsResponse = new GetLevelsResponse();
            levelsResponse.Levels = new List<Level>();

            foreach(var educationSubjectLevel in educationSubjectLevels)
             levelsResponse.Levels.Add(new Level
                {
                    WebShop = webShop,
                    AreaId = educationSubjectLevel.Parent.AreaId,
                    LevelNumber = educationSubjectLevel.Level,
                    Name = educationSubjectLevel.Name,
             });

            return levelsResponse;
        }
    }
}
