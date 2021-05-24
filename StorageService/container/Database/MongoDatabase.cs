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

        public async Task<NewPacket> Get(string deviceAddress)
        {
            _logger.LogDebug("Get request, deviceAddress: {@deviceAddress}", deviceAddress);
            var collection = GetNewPacketCollection();

            var filter = Builders<NewPacket>.Filter.Where(x => x.Uplink.decodedPacket.PhysicalPayload.MacPayload.FrameHeader.DeviceAddress == deviceAddress);

            var foundPacket = collection.Find(filter).FirstOrDefault();
            return foundPacket;
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
            var filter = Builders<NewPacket>.Filter.Eq("ReceivedPacket", packet.Uplink);
            await collection.DeleteOneAsync(filter);
        }
    }
}
