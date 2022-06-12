using System.Threading.Tasks;
using FluentAssertions;
using Gyldendal.Porter.Application.Contracts.Enums;
using Gyldendal.Porter.Application.Contracts.Request;
using Gyldendal.Porter.Application.Services.EducationSubjectLevel;
using Xunit;

namespace Gyldendal.Porter.Tests.IntegrationTests.PorterApi
{
    using static PorterApiTestSetup;
    public class EducationalSubjectLevelHandlersTests
    {
        [Fact]
        [Trait("Category", "IntegrationTest")]
        public async Task EducationalSubjectLevelQuery_Handler_ShouldSaveData()
        {
            new PorterApiTestSetup().RunBeforeQueryHandlerTests();

            var result = await SendAsync(new LevelFetchQuery(new GetLevelsRequest
            {
                WebShop = WebShop.GU,
                AreaId = null
            }));

            result.Levels.Should().NotBeNull();
        }
    }
}
