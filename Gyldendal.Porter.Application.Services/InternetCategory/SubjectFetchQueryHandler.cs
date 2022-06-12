using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Gyldendal.Porter.Application.Contracts.Response;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using MediatR;
using Gyldendal.Porter.Application.Contracts.Enums;
using Gyldendal.Porter.Application.Contracts.Models;

namespace Gyldendal.Porter.Application.Services.InternetCategory
{
    public class SubjectFetchQueryHandler : IRequestHandler<SubjectFetchQuery, GetSubjectsResponse>
    {
        private readonly IInternetCategoryRepository _internetCategoryRepository;

        public SubjectFetchQueryHandler(IInternetCategoryRepository internetCategoryRepository)
        {
            _internetCategoryRepository = internetCategoryRepository;
        }

        public async Task<GetSubjectsResponse> Handle(SubjectFetchQuery request, CancellationToken cancellationToken)
        {
            var internetCategories = await _internetCategoryRepository.GetSubjectsAsync(request.SubjectRequest.WebShop,request.SubjectRequest.areaId);
            return GetAreas(internetCategories, request.SubjectRequest.WebShop);
        }

        public GetSubjectsResponse GetAreas(List<Domain.Contracts.Entities.MasterData.InternetCategory> internetCategories, WebShop webShop)
        {
            GetSubjectsResponse subjectResponse = new GetSubjectsResponse();
            subjectResponse.Subjects = new List<Subject>();

            foreach(var subject in internetCategories)
             subjectResponse.Subjects.Add(new Subject
                {
                    Id = int.Parse(subject.Id),
                    Name = subject.Name,
                    WebShop = webShop,
                    AreaId = int.Parse(subject.Parent.Id)
             });

            return subjectResponse;
        }
    }
}
