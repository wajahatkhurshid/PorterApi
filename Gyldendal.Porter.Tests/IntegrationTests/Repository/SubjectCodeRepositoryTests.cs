using System.Threading.Tasks;
using FluentAssertions;
using Gyldendal.Porter.Infrastructure.Repository;
using Microsoft.Extensions.Caching.Memory;
using Xunit;

namespace Gyldendal.Porter.Tests.IntegrationTests.Repository
{
    public class SubjectCodeRepositoryTests
    {
        [Fact]
        public async Task GetSubjectCodeAsync_WithCache_EnsureCacheWorks()
        {
            var repository =
                new SubjectCodeRepository(IntegrationTestHelper.CreateNewMongoDbContext(), new MemoryCache(new MemoryCacheOptions()));

            var subjectCode = await repository.GetSubjectCodeAsync("ABC");
            
            var cachedSubjectCode = await repository.GetSubjectCodeAsync("ABC");

            subjectCode.Name.Should().BeEquivalentTo(cachedSubjectCode.Name);
        }
    }
}
