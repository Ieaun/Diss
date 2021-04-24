namespace container.Database
{
    using MongoDB.Driver;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using LoraWAN_Pipeline;
    using Microsoft.Extensions.Logging;

    public class MongoDatabase : IDatabase
    {
        private readonly MongoClient _mongoClient;
        private readonly ILogger<MongoDatabase> _logger;

        public MongoDatabase(ILogger<MongoDatabase> logger, MongoClient mongoClient)
        {
            this._logger = logger;
            this._mongoClient = mongoClient;
        }

        public async Task Create(StubObject stubObject)
        {
            var collection = _mongoClient.GetDatabase($"{nameof(StubObject)}").GetCollection<StubObject>($"{nameof(StubObject)}");
            await collection.InsertOneAsync(stubObject);
        }
        public async Task<StubObject> Get(int Id)
        {
            var collection = _mongoClient.GetDatabase($"{nameof(StubObject)}").GetCollection<StubObject>($"{nameof(StubObject)}");
            var stubObject = await collection.Find(x => x.Id == Id).FirstOrDefaultAsync();
            return stubObject;
        }

        public async Task<IEnumerable<StubObject>> GetAll()
        {
            var collection = _mongoClient.GetDatabase($"{nameof(StubObject)}").GetCollection<StubObject>($"{nameof(StubObject)}");
            var stubObjects = await collection.Find(_ => true).ToListAsync();
            return stubObjects;
        }

        public async Task Delete(StubObject stubObject)
        {
            var collection = _mongoClient.GetDatabase($"{nameof(StubObject)}").GetCollection<StubObject>($"{nameof(StubObject)}");
            var filter = Builders<StubObject>.Filter.Eq("Id", stubObject.Id);
            await collection.DeleteOneAsync(filter);
        }

        public async Task Update(StubObject stubObject)
        {
            var collection = _mongoClient.GetDatabase($"{nameof(StubObject)}").GetCollection<StubObject>($"{nameof(StubObject)}");
            await collection.ReplaceOneAsync(doc => doc.Id == stubObject.Id, stubObject);
        }
    }
}
