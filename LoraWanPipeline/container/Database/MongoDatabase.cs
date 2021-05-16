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
        public async Task<RegisteredDevice> Get(string DeviceAddress)
        {
            _logger.LogDebug("Get request, Device Address: {@DeviceAddress}", DeviceAddress);
            var collection = GetCollection();

            var filter = Builders<RegisteredDevice>.Filter.Where(x => x.DeviceAddress == DeviceAddress);

            var device = collection.Find(filter).FirstOrDefault();
            return device;
        }

        public async Task<List<RegisteredDevice>> GetAll()
        {
            _logger.LogDebug("GetAll request");
            var collection = GetCollection();
            var device = await collection.Find(_ => true).ToListAsync();
            return device;
        }

        public async Task Delete(RegisteredDevice registeredDevice)
        {
            _logger.LogDebug("Delete request, deleting: {@registeredDevice}", registeredDevice);
            var collection = GetCollection();
            var filter = Builders<RegisteredDevice>.Filter.Eq("DeviceAddress", registeredDevice.DeviceAddress);
            await collection.DeleteOneAsync(filter);
        }

        public async Task Update(RegisteredDevice registeredDevice)
        {
            _logger.LogDebug("Update request, update: {@registeredDevice}", registeredDevice);
            var collection = GetCollection();
            await collection.ReplaceOneAsync(doc => doc.DeviceAddress == registeredDevice.DeviceAddress, registeredDevice);
        }
    }
}
