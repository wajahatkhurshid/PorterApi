using FluentAssertions;
using Gyldendal.Porter.Domain.Contracts.Entities;
using Gyldendal.Porter.Infrastructure.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gyldendal.Porter.Application.Contracts.Enums;
using Xunit;

namespace Gyldendal.Porter.Tests.IntegrationTests.Repository
{
    public class WorkReviewRepositoryTests
    {
        [Fact]
        public async Task ShouldMapAndSaveWorkReviewsCorrectly()
        {
            var workReview = new WorkReview()
            {
                Id = "1234",
                AuthorInfo = "Henning Mortensen er født i 1939.",
                UpdatedTimestamp = System.DateTime.Now,
                Rating = 4,
                Review = "Ida Jessen rører os igen",
                Title = "Et overmåde spændende portræ",
                WebShopIds = new List<string> { WebShop.GU.ToString(), WebShop.MunksGaard.ToString(), WebShop.HansReitzel.ToString() },
                WorkId = "1",
                WorkReviewId = "1",
                IsDeleted = false

            };
            var repository = new WorkReviewRepository(IntegrationTestHelper.CreateNewMongoDbContext(), IntegrationTestHelper.CreateMapper());

            await repository.DeleteAllAsync();

            await repository.UpsertWorkReviewAsync(workReview);

            var workReviews = await repository.GetAllAsync();

            workReviews.Count.Should().Be(1, "All workReviews were cleared prior to test and only one added afterwards");

            var savedworkReview = workReviews.First();

            savedworkReview.Id.Should().Be(workReview.Id, "WorkReview properties should be identical after fetching");

            savedworkReview.AuthorInfo.Should().Be(workReview.AuthorInfo);

            savedworkReview.WebShopIds.Count.Should().Be(3, "3 WebShop Ids are added");

            await repository.DeleteAllAsync();

            var emptyworkReviewList = await repository.GetAllAsync();

            emptyworkReviewList.Count.Should().Be(0, "Deleted the sole workReview left");
        }
    }
}
