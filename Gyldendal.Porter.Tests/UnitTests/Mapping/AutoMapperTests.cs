using AutoMapper;
using Gyldendal.Porter.Application.Configuration.AutoMapper;
using Xunit;

namespace Gyldendal.Porter.Tests.UnitTests.Mapping
{
    public class AutoMapperTests
    {
        [Fact]
        public void Validate_AutoMapper_Configuration()
        {
            var config = new MapperConfiguration(cfg => { cfg.AddProfile<PorterMapper>(); });
            config.AssertConfigurationIsValid();
        }
    }
}
