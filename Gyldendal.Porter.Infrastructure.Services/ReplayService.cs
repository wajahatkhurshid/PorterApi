using System.Linq;
using System.Threading.Tasks;
using Gyldendal.Porter.Domain.Contracts.Interfaces;
using Gyldendal.Porter.Infrastructure.ExternalClients.Interfaces;
using Gyldendal.Porter.Infrastructure.Repository;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Gyldendal.Porter.Infrastructure.Services
{
    public class ReplayService : IReplayService
    {
        private readonly IGpmApiClient _gpmApiClient;
        private readonly PorterContext _context;

        public ReplayService(IGpmApiClient gpmApiClient, PorterContext context)
        {
            _gpmApiClient = gpmApiClient;
            _context = context;
        }

        /// <summary>
        /// Clears all Mongo collections in preparation for a fresh load of data
        /// </summary>
        /// <param name="subscriptionId">GPMSubscription ID to trigger a replay for</param>
        /// <param name="shouldWipeCollections">Indicates if the collections should cleared as part of the replay trigger</param>
        /// <returns>True if the replay was triggered successfully on the GPM API</returns>
        public async Task<bool> TriggerReplayAsync(int subscriptionId, bool shouldWipeCollections)
        {
            if (shouldWipeCollections)
            {
                var database = _context.Db;
                var collectionCursor = await database.ListCollectionsAsync();
                var collections = await collectionCursor.ToListAsync();

                foreach (var collectionMetaData in collections)
                {
                    var collectionNameElement = collectionMetaData.Elements.First(v => v.Name.Equals("name"));
                    var collectionName = collectionNameElement.Value.AsString;
                    // Private collections usually start with an underscore.
                    // We don't want to wipe the migrations collection for example.
                    // Neither the Hangfire state.
                    if (!collectionName.StartsWith("_") && !collectionName.Contains("hangfire") && !collectionName.Contains("Subscription"))
                    {
                        //if (collectionName.Contains("EducationSubjectLevel") || collectionName.Contains("Imprint") || collectionName.Contains("InternetCategory") || collectionName.Contains("MediaMaterialType")
                        //    || collectionName.Contains("SubjectCode") || collectionName.Contains("SupplyAvailability"))
                        //    continue;

                        var col = _context.Db.GetCollection<BsonDocument>(collectionName);
                        await col.DeleteManyAsync(new FilterDefinitionBuilder<BsonDocument>().Empty);
                    }
                }
            }

            return await _gpmApiClient.TriggerReplayAsync(subscriptionId);
        }
    }
}