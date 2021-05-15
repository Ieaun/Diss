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

        private IMongoCollection<RegisteredDevice> GetCollection() => _mongoClient.GetDatabase($"{nameof(RegisteredDevice)}").GetCollection<RegisteredDevice>($"{nameof(RegisteredDevice)}");

        public async Task Create(RegisteredDevice registeredDevice)
        {
            _logger.LogDebug("Create request, creating: {@registeredDevice}", registeredDevice);
            var collection = GetCollection();
            await collection.InsertOneAsync(registeredDevice);

        }
        public async Task<RegisteredDevice> Get(int id)
        {
            _logger.LogDebug("Get request, id: {@Id}", id);
            var collection = GetCollection();

            var filter = Builders<RegisteredDevice>.Filter.Where(x => x.Id == id);

            var stubObject = collection.Find(filter).FirstOrDefault();
            return stubObject;
        }

        public async Task<List<RegisteredDevice>> GetAll()
        {
            _logger.LogDebug("GetAll request");
            var collection = GetCollection();
            var stubObjects = await collection.Find(_ => true).ToListAsync();
            return stubObjects;
        }

        public async Task Delete(RegisteredDevice registeredDevice)
        {
            _logger.LogDebug("Delete request, deleting: {@registeredDevice}", registeredDevice);
            var collection = GetCollection();
            var filter = Builders<RegisteredDevice>.Filter.Eq("Id", registeredDevice.Id);
            await collection.DeleteOneAsync(filter);
        }

        public async Task Update(RegisteredDevice registeredDevice)
        {
            _logger.LogDebug("Update request, update: {@registeredDevice}", registeredDevice);
            var collection = GetCollection();
            await collection.ReplaceOneAsync(doc => doc.Id == registeredDevice.Id, registeredDevice);
        }
    }
}
