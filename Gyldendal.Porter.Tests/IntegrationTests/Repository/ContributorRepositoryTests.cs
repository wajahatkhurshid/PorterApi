using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Gyldendal.Porter.Application.Configuration.AutoMapper;
using Gyldendal.Porter.Domain.Contracts.Entities;
using Gyldendal.Porter.Infrastructure.Repository;
using Xunit;

namespace Gyldendal.Porter.Tests.IntegrationTests.Repository
{
    public class ContributorRepositoryTests
    {
        [Fact]
        public async Task ShouldMapAndSaveContributorsCorrectly()
        {
            var config = new MapperConfiguration(cfg => { cfg.AddProfile<PorterMapper>(); });

            var mapper = config.CreateMapper();
            var contributor = new Contributor()
            {
                Id = "1234",
                BiographyText = "Biography",
                ContributorTypeId = 7,
                FirstName = "A",
                LastName = "B",
                IsDeleted = false,
                PhotoUrl = "https",
                UpdatedTimestamp = DateTime.UtcNow,
            };
            var repository = new ContributorRepository(IntegrationTestHelper.CreateNewMongoDbContext());

            await repository.DeleteAllContributorsAsync();

            await repository.UpsertAsync(contributor);

            var contributors = await repository.GetListAsync();

            contributors.Count.Should().Be(1, "All contributors were cleared prior to test and only one added afterwards");
            var savedContributor = contributors.First();

            savedContributor.Id.Should().Be(contributor.Id, "Contributor properties should be identical after fetching");
            savedContributor.BiographyText.Should().Be(contributor.BiographyText);

            await repository.DeleteAsync(savedContributor.Id);

            var emptyContributorsList = await repository.GetListAsync();
            emptyContributorsList.Count.Should().Be(0, "Deleted the sole contributor left");
        }
    }
}