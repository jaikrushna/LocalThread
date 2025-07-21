using LocalThreads.Api.Configurations;
using LocalThreads.Api.Models.Shopkeeper;
using LocalThreads.Api.Repositories.Interfaces.Shopkeeper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LocalThreads.Api.Repositories.Implementations.Shopkeeper
{
    public class ShopRepository : IShopRepository
    {
        private readonly IMongoCollection<Shop> _shops;

        public ShopRepository(IOptions<MongoDbSettings> mongoSettings)
        {
            var client = new MongoClient(mongoSettings.Value.ConnectionString);
            var database = client.GetDatabase(mongoSettings.Value.Database);
            _shops = database.GetCollection<Shop>("shops");
        }
        public async Task<Shop> GetByFirebaseUidAsync(string firebaseUid)
        {
            return await _shops.Find(shop => shop.FirebaseUid == firebaseUid).FirstOrDefaultAsync();
        }
        public async Task<Shop> CreateShopAsync(Shop shop)
        {
            await _shops.InsertOneAsync(shop);
            return shop;
        }

        public async Task<Shop> GetByPhoneNumberAsync(string phoneNumber)
        {
            return await _shops.Find(s => s.PhoneNumber == phoneNumber).FirstOrDefaultAsync();
        }

        public async Task<Shop> GetByIdAsync(string shopId)
        {
            return await _shops.Find(s => s.Id == shopId).FirstOrDefaultAsync();
        }

        public async Task UpdateShopImageUrlsAsync(string shopId, string shopImageUrl, string governmentIdUrl)
        {
            var filter = Builders<Shop>.Filter.Eq(s => s.Id, shopId);
            var update = Builders<Shop>.Update
                .Set(s => s.ShopImageUrl, shopImageUrl)
                .Set(s => s.GovernmentIdUrl, governmentIdUrl);

            await _shops.UpdateOneAsync(filter, update);
        }
    }
}
