using Gyldendal.Porter.Application.Contracts.Request;
using Gyldendal.Porter.Application.Contracts.Response;
using MediatR;

namespace Gyldendal.Porter.Application.Services.InternetCategory
{
    public class SubjectFetchQuery : IRequest<GetSubjectsResponse>
    {
        public GetSubjectsRequest SubjectRequest { get; set; }

        public SubjectFetchQuery(GetSubjectsRequest subjectRequest)
        {
            SubjectRequest = subjectRequest;
        }
    }
}
