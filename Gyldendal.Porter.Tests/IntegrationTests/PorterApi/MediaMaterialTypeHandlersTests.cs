using System.Threading.Tasks;
using FluentAssertions;
using Gyldendal.Porter.Application.Services.MediaMaterialType;
using Xunit;

namespace Gyldendal.Porter.Tests.IntegrationTests.PorterApi
{
    using static PorterApiTestSetup;
    public class MediaMaterialTypeHandlersTests
    {
        [Fact]
        [Trait("Category", "IntegrationTest")]
        public async Task MaterialTypeFetchQuery_Handler_ShouldGetData()
        {
            new PorterApiTestSetup().RunBeforeQueryHandlerTests();
            var result = await SendAsync(new MaterialTypeFetchQuery());
            result.MaterialTypes.Should().NotBeNull();
        }

        [Fact]
        [Trait("Category", "IntegrationTest")]
        public async Task MediaTypeFetchQuery_Handler_ShouldGetData()
        {
            new PorterApiTestSetup().RunBeforeQueryHandlerTests();
            var result = await SendAsync(new MediaTypeFetchQuery());
            result.MediaTypes.Should().NotBeNull();
        }

    }
}
