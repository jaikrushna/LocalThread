using MongoDB.Driver;
using LocalThreads.Api.Models.Shopkeeper;
using LocalThreads.Models.Customer.Landing;

namespace LocalThreads.Api.Services.Interfaces.Shared
{
    public interface IMongoDbService
    {
        IMongoCollection<Banner> Banners { get; }
        IMongoCollection<Category> Categories { get; }
        IMongoCollection<Product> Products { get; }
        IMongoCollection<Shop> Shops { get; }

    }
}
