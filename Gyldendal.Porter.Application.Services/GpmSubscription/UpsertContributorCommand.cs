using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using Gyldendal.Porter.Domain.Contracts.ValueObjects.Containers;
using MediatR;

namespace Gyldendal.Porter.Application.Services.GpmSubscription
{
    public class UpsertContributorCommand : IRequest<bool>
    {
        public ProfileContributorAuthorContainer ContributorContainer { get; set; }

        public class UpsertContributorCommandHandler : IRequestHandler<UpsertContributorCommand, bool>
        {
            private readonly IContributorRepository _contributorRepository;
            private readonly IMapper _mapper;

            public UpsertContributorCommandHandler(IContributorRepository contributorRepository, IMapper mapper)
            {
                _contributorRepository = contributorRepository;
                _mapper = mapper;
            }

            public async Task<bool> Handle(UpsertContributorCommand request, CancellationToken cancellationToken)
            {
                await _contributorRepository.UpsertAsync(
                    _mapper.Map<Domain.Contracts.Entities.Contributor>(request.ContributorContainer));
                /*await _contributorRepository.UpsertAsync(new Domain.Contracts.Entities.Contributor
                {
                    UpdatedTimestamp = DateTime.Now,
                    BiographyText = container.ProfileText,
                    Id = container.ProfileID.ToString(),
                    FirstName = container.ProfileName.Split(" ")[0], // TODO: Need some proper name splitting here
                    LastName = container.ProfileName.Split(" ").Length > 1 ? container.ProfileName.Split(" ")[1] : string.Empty,
                    ContributorTypeId = 0, // TODO: Still missing
                    IsDeleted = false, // TODO: Still missing
                    PhotoUrl = container.ProfileImageUrl,
                    Bibliography = string.Empty // TODO: Still needs to be added
                });*/

                return true;
            }
        }
    }
}