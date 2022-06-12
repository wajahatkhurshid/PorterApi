using System.Threading.Tasks;
using FluentAssertions;
using Gyldendal.Porter.Application.Contracts.Enums;
using Gyldendal.Porter.Application.Contracts.Request;
using Gyldendal.Porter.Application.Services.InternetCategory;
using Xunit;

namespace Gyldendal.Porter.Tests.IntegrationTests.PorterApi
{
    using static PorterApiTestSetup;
    public class InternetCategoryHandlersTests
    {
        [Fact]
        [Trait("Category", "IntegrationTest")]
        public async Task AreaFetchQuery_Handler_ShouldGetData()
        {
            new PorterApiTestSetup().RunBeforeQueryHandlerTests();

            var result = await SendAsync(new AreaFetchQuery(WebShop.GU));

            result.Areas.Should().NotBeNull();
        }

        [Fact]
        [Trait("Category", "IntegrationTest")]
        public async Task SubAreaFetchQuery_Handler_ShouldGetData()
        {
            new PorterApiTestSetup().RunBeforeQueryHandlerTests();

            var result = await SendAsync(new SubAreaFetchQuery(new GetSubAreasRequest
            {
                WebShop = WebShop.GU,
                SubjectId = null
            }));

            result.SubAreas.Should().NotBeNull();
        }

        [Fact]
        [Trait("Category", "IntegrationTest")]
        public async Task SubjectFetchQuery_Handler_ShouldGetData()
        {
            new PorterApiTestSetup().RunBeforeQueryHandlerTests();

            var result = await SendAsync(new SubjectFetchQuery(new GetSubjectsRequest
            {
                WebShop = WebShop.GU,
                areaId = null
            }));

            result.Subjects.Should().NotBeNull();
        }

    }
}
