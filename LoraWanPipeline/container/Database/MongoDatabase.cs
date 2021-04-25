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
            _logger.LogDebug("Create request, creating: {@stubObject}", stubObject);
            var collection = _mongoClient.GetDatabase($"{nameof(StubObject)}").GetCollection<StubObject>($"{nameof(StubObject)}");
            await collection.InsertOneAsync(stubObject);

        }
        public async Task<StubObject> Get(int id)
        {
            _logger.LogDebug("Get request, id: {@Id}", id);
            var collection = _mongoClient.GetDatabase($"{nameof(StubObject)}").GetCollection<StubObject>($"{nameof(StubObject)}");

            var filter = Builders<StubObject>.Filter.Where(x => x.Id == id);

            var stubObject = collection.Find(filter).FirstOrDefault();
            return stubObject;
        }

        public async Task<List<StubObject>> GetAll()
        {
            _logger.LogDebug("GetAll request");
            var collection = _mongoClient.GetDatabase($"{nameof(UplinkService.StubObject)}").GetCollection<StubObject>($"{nameof(StubObject)}");
            var stubObjects = await collection.Find(_ => true).ToListAsync();
            return stubObjects;
        }

        public async Task Delete(StubObject stubObject)
        {
            _logger.LogDebug("Delete request, deleting: {@StubObject}", stubObject);
            var collection = _mongoClient.GetDatabase($"{nameof(StubObject)}").GetCollection<StubObject>($"{nameof(StubObject)}");
            var filter = Builders<StubObject>.Filter.Eq("Id", stubObject.Id);
            await collection.DeleteOneAsync(filter);
        }

        public async Task Update(StubObject stubObject)
        {
            _logger.LogDebug("Update request, update: {@StubObject}", stubObject);
            var collection = _mongoClient.GetDatabase($"{nameof(StubObject)}").GetCollection<StubObject>($"{nameof(StubObject)}");
            await collection.ReplaceOneAsync(doc => doc.Id == stubObject.Id, stubObject);
        }
    }
}
