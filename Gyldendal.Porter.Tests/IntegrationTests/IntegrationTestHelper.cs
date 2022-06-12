using System;
using System.Net.Http;
using AutoMapper;
using Gyldendal.Porter.Application.Configuration.AutoMapper;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using Gyldendal.Porter.Infrastructure.ExternalClients.Gpm;
using Gyldendal.Porter.Infrastructure.Repository;

namespace Gyldendal.Porter.Tests.IntegrationTests
{
    public static class IntegrationTestHelper
    {
        public static PorterContext CreateNewMongoDbContext()
        {
            return new PorterContext("mongodb://localhost/?maxPoolSize=6000", "PorterDb");
        }

        public static GpmApiClient CreateNewGpmApiClient(ITaxonomyResponseRepository taxonomyResponseRepository = null)
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://test-gpmapi.gyldendal.local")
            };
            var configuration = new GpmConfiguration();
            return new GpmApiClient(httpClient, configuration, taxonomyResponseRepository);
        }
        public static IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<PorterMapper>();
            });

            return config.CreateMapper();
        }
    }
}
