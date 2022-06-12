using Gyldendal.Porter.Application.Contracts.Request;
using Gyldendal.Porter.Application.Contracts.Response;
using MediatR;

namespace Gyldendal.Porter.Application.Services.EducationSubjectLevel
{
    public class LevelFetchQuery : IRequest<GetLevelsResponse>
    {
        public GetLevelsRequest LevelsRequest { get; set; }

        public LevelFetchQuery(GetLevelsRequest levelsRequest)
        {
            LevelsRequest = levelsRequest;
        }
    }
}
