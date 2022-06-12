using System;
using System.Threading;
using System.Threading.Tasks;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using Gyldendal.Porter.Domain.Contracts.ValueObjects.Containers;
using MediatR;

namespace Gyldendal.Porter.Application.Services.GpmSubscription
{
    public class UpsertWorkCommand : IRequest<bool>
    {
        public WorkContainer WorkContainer { get; set; }

        public class UpsertWorkCommandHandler : IRequestHandler<UpsertWorkCommand, bool>
        {
            private readonly IWorkRepository _workRepository;

            public UpsertWorkCommandHandler(IWorkRepository workRepository)
            {
                _workRepository = workRepository;
            }

            public async Task<bool> Handle(UpsertWorkCommand request, CancellationToken cancellationToken)
            {
                await _workRepository.UpsertWorkAsync(new Domain.Contracts.Entities.Work
                {
                    Id = request.WorkContainer.WorkIdentificationNumber,
                    ContainerInstanceId = request.WorkContainer.ContainerInstanceId,
                    Title = request.WorkContainer.WorkTitle,
                    Subtitle = request.WorkContainer.WorkSubtitle,
                    WorkWebText = request.WorkContainer.WorkWebText,
                    WorkGyldendalShopText = request.WorkContainer.WorkGyldendalShopText,
                    WorkInternalText = request.WorkContainer.WorkInternalText,
                    ProductIds = request.WorkContainer.ProductIds,
                    WorkSubjectCodeMain = request.WorkContainer.WorkSubjectCodeMain,
                    WorkSubjectCode = request.WorkContainer.WorkSubjectCode,
                    WorkGUInternetSubject = request.WorkContainer.WorkGUInternetSubject,
                    WorkEducationSubjectLevel = request.WorkContainer.WorkEducationSubjectLevel,
                    WorkReviews = request.WorkContainer.WorkReviews,
                    UpdatedTimestamp = DateTime.UtcNow
                });

                return true;
            }
        }
    }
}