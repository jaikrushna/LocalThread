using System.Text.RegularExpressions;
using LocalThreads.Api.Configurations;
using LocalThreads.Api.Repositories.Interfaces.Customer;
using LocalThreads.Models.Customer.Landing;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace LocalThreads.Api.Repositories.Implementations.Customer
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IMongoCollection<Category> _categoryCollection;

        public CategoryRepository(IOptions<MongoDbSettings> mongoSettings)
        {
            var client = new MongoClient(mongoSettings.Value.ConnectionString);
            var database = client.GetDatabase(mongoSettings.Value.Database);

            _categoryCollection = database.GetCollection<Category>("categories");
        }

        public async Task<Category> GetByNameAsync(string categoryName)
        {
            var filter = Builders<Category>.Filter.Regex(
                c => c.Name,
                new BsonRegularExpression($"^{Regex.Escape(categoryName)}$", "i")  // "i" for case-insensitive
            );

            return await _categoryCollection.Find(filter).FirstOrDefaultAsync();
        }
    }
}
