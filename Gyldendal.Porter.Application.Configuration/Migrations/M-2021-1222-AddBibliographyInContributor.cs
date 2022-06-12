using MongoDB.Bson;
using MongoDB.Driver;
using RapidCore.Migration;
using RapidCore.Mongo.Migration;

namespace Gyldendal.Porter.Application.Configuration.Migrations
{
    public class M_2021_1222_AddBibliographyInContributor : MigrationBase
    {
        protected override void ConfigureUpgrade(IMigrationBuilder builder)
        {
            builder.Step("Added Bibliography In Contributor", async () =>
            {
                var ctx = base.ContextAs<MongoMigrationContext>();
                var db = ctx.ConnectionProvider.Default();

                var collection = db.GetCollection<BsonDocument>("Contributor");

                var postCode = Builders<BsonDocument>.Update.Set("Bibliography", "");

                await collection.UpdateManyAsync(_ => true, postCode);

            });
        }

        protected override void ConfigureDowngrade(IMigrationBuilder builder)
        {
            throw new System.NotImplementedException();
        }
    }
}

