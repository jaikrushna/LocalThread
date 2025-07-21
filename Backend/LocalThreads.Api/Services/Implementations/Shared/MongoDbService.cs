using LocalThreads.Api.Models;
using LocalThreads.Api.Configurations;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using LocalThreads.Api.Services.Interfaces.Shared;
using LocalThreads.Api.Models.Shopkeeper;
using LocalThreads.Models.Customer.Landing;

namespace LocalThreads.Api.Services.Implementations.Shared
{

    public class MongoDbService : IMongoDbService
    {
        private readonly IMongoDatabase _database;

        public MongoDbService(IOptions<MongoDbSettings> settings)
        {
            var dbSettings = settings.Value;
            var client = new MongoClient(dbSettings.ConnectionString);
            _database = client.GetDatabase(dbSettings.Database);
        }

        public IMongoCollection<Banner> Banners =>
            _database.GetCollection<Banner>("banners");

        public IMongoCollection<Category> Categories =>
            _database.GetCollection<Category>("categories");

        public IMongoCollection<Product> Products =>
            _database.GetCollection<Product>("products");

        public IMongoCollection<Shop> Shops =>
            _database.GetCollection<Shop>("shops");

    }
}
