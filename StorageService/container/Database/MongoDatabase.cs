namespace StorageService.Database
{
    using MongoDB.Driver;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using StorageService;
    using Microsoft.Extensions.Logging;
    using StorageService.Notifications.ReceivedPackets;

    public class MongoDatabase : IDatabase
    {
        private readonly MongoClient _mongoClient;
        private readonly ILogger<MongoDatabase> _logger;

        public MongoDatabase(ILogger<MongoDatabase> logger, MongoClient mongoClient)
        {
            this._logger = logger;
            this._mongoClient = mongoClient;
        }

        private IMongoCollection<NewPacket> GetNewPacketCollection() => _mongoClient.GetDatabase($"{nameof(NewPacket)}").GetCollection<NewPacket>($"{nameof(NewPacket)}");

        public async Task Create(NewPacket packet)
        {
            _logger.LogDebug("Create request, creating: {@packet}", packet);
            var collection = GetNewPacketCollection();
            await collection.InsertOneAsync(packet);

        }
        public async Task<NewPacket> Get(int id)
        {
            _logger.LogDebug("Get request, id: {@Id}", id);
            var collection = GetNewPacketCollection();

            var filter = Builders<NewPacket>.Filter.Where(x=> x.Id == id);

            var stubObject = collection.Find(filter).FirstOrDefault();
            return stubObject;
        }

        public async Task<List<NewPacket>> GetAll()
        {
            _logger.LogDebug("GetAll request");
            var collection = GetNewPacketCollection();
            var packetsList = await collection.Find(_ => true).ToListAsync();
            return packetsList;
        }

        public async Task Delete(NewPacket packet)
        {
            _logger.LogDebug("Delete request, deleting: {@NewPacket}", packet);
            var collection = GetNewPacketCollection();
            var filter = Builders<NewPacket>.Filter.Eq("Id", packet.Id);
            await collection.DeleteOneAsync(filter);
        }

        public async Task Update(NewPacket packet)
        {
            _logger.LogDebug("Update request, update: {@NewPacket}", packet);
            var collection = GetNewPacketCollection();
            await collection.ReplaceOneAsync(doc => doc.Id == packet.Id, packet);
        }
    }
}
