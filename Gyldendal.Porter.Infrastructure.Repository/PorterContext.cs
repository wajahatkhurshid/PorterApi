using MongoDB.Driver;

namespace Gyldendal.Porter.Infrastructure.Repository
{
    public class PorterContext
    {
        public readonly IMongoDatabase Db;

        public PorterContext(IMongoDatabase database)
        {
            Db = database;
        }

        public PorterContext(string connectionString, string dbName)
        {
            var client = new MongoClient(connectionString);
            Db = client.GetDatabase(dbName);
        }
    }
}
