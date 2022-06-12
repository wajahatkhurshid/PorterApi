using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Gyldendal.Porter.Application.Configuration.AutoMapper;
using Gyldendal.Porter.Domain.Contracts.Entities;
using Gyldendal.Porter.Domain.Contracts.ValueObjects.Containers;
using Gyldendal.Porter.Infrastructure.Repository;
using Xunit;

namespace Gyldendal.Porter.Tests.IntegrationTests.Repository
{
    public class WorkRepositoryTest
    {
        [Fact]
        public async Task ShouldMapAndSaveWorksCorrectly()
        {
            var config = new MapperConfiguration(cfg => { cfg.AddProfile<PorterMapper>(); });

            var mapper = config.CreateMapper();
            var work = new Work
            {
                Id = "1234",
                Title = "Biography",
                WorkWebText = "Testing",
                ProductIds = new List<int>() { 1, 2 },
                WorkSubjectCode = new List<List<GpmNode>>() { new List<GpmNode> { new GpmNode { NodeId = 1, Name = "Value" } } },
                WorkGUInternetSubject = new List<List<GpmNode>>() { new List<GpmNode> { new GpmNode { NodeId = 1, Name = "Value" } } },
                WorkEducationSubjectLevel = new List<List<GpmNode>>() { new List<GpmNode> { new GpmNode { NodeId = 1, Name = "Value" } } }
            };
            var repository = new WorkRepository(IntegrationTestHelper.CreateNewMongoDbContext());

            await repository.DeleteAllAsync();

            await repository.UpsertWorkAsync(work);

            var works = await repository.GetWorksAsync();

            works.Count.Should().Be(1, "All works were cleared prior to test and only one added afterwards");

            var savedWork = works.First();

            savedWork.Id.Should().Be(work.Id, "work properties should be identical after fetching");

            savedWork.Title.Should().Be(work.Title);

            savedWork.ProductIds.Should().BeEquivalentTo(work.ProductIds);

            savedWork.WorkSubjectCode.Count.Should().Be(1, "Only one Area is added in work");

            savedWork.WorkGUInternetSubject.Count.Should().Be(1, "Only one EducationLevel is added in work");

            savedWork.WorkEducationSubjectLevel.Count.Should().Be(1, "Only one Subject is added in work");

            await repository.DeleteWorkAsync(savedWork.Id);

            var emptyWorkList = await repository.GetWorksAsync();

            emptyWorkList.Count.Should().Be(0, "Deleted the sole contributor left");
        }
    }
}
