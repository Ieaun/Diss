namespace UplinkService.Database
{
    using MongoDB.Driver;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using UplinkService;
    using Microsoft.Extensions.Logging;
    using UplinkService.Models.ReceivedPackets;
    using UplinkService.Models.Servers;

    public class MongoDatabase : IDatabase
    {
        private readonly MongoClient _mongoClient;
        private readonly ILogger<MongoDatabase> _logger;

        public MongoDatabase(ILogger<MongoDatabase> logger, MongoClient mongoClient)
        {
            this._logger = logger;
            this._mongoClient = mongoClient;
        }

        private IMongoCollection<Server> GetCollection() => _mongoClient.GetDatabase($"{nameof(Server)}").GetCollection<Server>($"{nameof(Server)}");

        public async Task Create(Server serverObject)
        {
            _logger.LogDebug("Create request, creating: {@Server}", serverObject);
            var collection = GetCollection();
            await collection.InsertOneAsync(serverObject);

        }
        public async Task<Server> Get(int id)
        {
            _logger.LogDebug("Get request, id: {@Id}", id);
            var collection = GetCollection();

            var filter = Builders<Server>.Filter.Where(x=> x.Id == id);

            var serverObject = collection.Find(filter).FirstOrDefault();
            return serverObject;
        }

        public async Task<List<Server>> GetAll()
        {
            _logger.LogDebug("GetAll request");
            var collection = GetCollection();
            var serverObjects = await collection.Find(_ => true).ToListAsync();
            return serverObjects;
        }

        public async Task Delete(Server serverObject)
        {
            _logger.LogDebug("Delete request, deleting: {@Server}", serverObject);
            var collection = GetCollection();
            var filter = Builders<Server>.Filter.Eq("Id", serverObject.Id);
            await collection.DeleteOneAsync(filter);
        }

        public async Task Update(Server serverObject)
        {
            _logger.LogDebug("Update request, update: {@Server}", serverObject);
            var collection = GetCollection();
            await collection.ReplaceOneAsync(doc => doc.Id == serverObject.Id, serverObject);
        }
    }
}
