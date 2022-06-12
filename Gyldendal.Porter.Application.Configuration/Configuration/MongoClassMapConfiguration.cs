using System;
using Gyldendal.Porter.Domain.Contracts.Entities;
using Gyldendal.Porter.Domain.Contracts.Entities.Cooked;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Gyldendal.Porter.Api.Configuration
{
    /// <summary>
    /// Contains Mongo class mapping functionality
    /// </summary>
    public static class MongoClassMapConfiguration
    {
        /// <summary>
        /// Configures domain entities to map their properties to Mongo indexes etc. without adding Mongo dependencies directly in the domain
        /// </summary>
        public static void UsePorterMongoClassMaps()
        {
            BsonClassMap.RegisterClassMap<DomainEntityBase>(cm =>
            {
                cm.MapIdMember(p => p.Id);
            });

            BsonClassMap.RegisterClassMap<CookedWorkReview>(cm =>
            {
                cm.AutoMap();
                cm.MapMember(c => c.UpdatedTimestamp).SetSerializer(new DateTimeSerializer(DateTimeKind.Utc));
                cm.SetIgnoreExtraElements(true);
            });

            BsonClassMap.RegisterClassMap<CookedProduct>(cm =>
            {
                cm.AutoMap();
                cm.MapMember(c => c.UpdatedTimestamp).SetSerializer(new DateTimeSerializer(DateTimeKind.Utc));
            });

            BsonClassMap.RegisterClassMap<CookedContributor>(cm =>
            {
                cm.AutoMap();
                cm.MapMember(c => c.UpdatedTimestamp).SetSerializer(new DateTimeSerializer(DateTimeKind.Utc));
            });
        }
    }
}
