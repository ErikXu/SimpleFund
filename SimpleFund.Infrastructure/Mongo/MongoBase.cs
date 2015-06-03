using MongoDB.Driver;

namespace SimpleFund.Infrastructure.Mongo
{
    public class MongoBase
    {
        protected MongoDatabase Init()
        {
            if (MongoSetting.Current == null)
            {
                throw new MongoException("No mongo setting info is provided.");
            }
            var client = new MongoClient(MongoSetting.Current.ConnectionString);
            var server = client.GetServer();
            var db = server.GetDatabase(MongoSetting.Current.DatabaseName);
            return db;
        }
    }
}
