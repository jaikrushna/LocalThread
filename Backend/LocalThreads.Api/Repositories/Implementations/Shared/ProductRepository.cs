using LocalThreads.Api.Configurations;
using LocalThreads.Api.Repositories.Interfaces.Shared;
using LocalThreads.Models.Customer.Landing;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Globalization;

namespace LocalThreads.Api.Repositories.Implementations.Shared
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoCollection<Product> _productCollection;
        private readonly IMongoCollection<Category> _categoryCollection;

        public ProductRepository(IOptions<MongoDbSettings> mongoSettings)
        {
            var client = new MongoClient(mongoSettings.Value.ConnectionString);
            var database = client.GetDatabase(mongoSettings.Value.Database);

            _productCollection = database.GetCollection<Product>("products");
            _categoryCollection = database.GetCollection<Category>("categories");
        }

        public async Task<string> CreateAsync(Product product)
        {
            await _productCollection.InsertOneAsync(product);

            var categoryName = product.Category.Trim().ToLowerInvariant();

            var category = await _categoryCollection
                .Find(c => c.Name.ToLower() == categoryName)
                .FirstOrDefaultAsync();

            if (category == null)
            {
                var newCategory = new Category
                {
                    Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(categoryName),
                    ProductCount = 1,
                    ProductIds = new List<string> { product.Id },
                    CreatedAt = DateTime.UtcNow
                };

                await _categoryCollection.InsertOneAsync(newCategory);
            }
            else
            {
                var update = Builders<Category>.Update
                    .Inc(c => c.ProductCount, 1)
                    .Push(c => c.ProductIds, product.Id);

                await _categoryCollection.UpdateOneAsync(c => c.Id == category.Id, update);
            }

            return product.Id;
        }

        public async Task<Product> GetByIdAsync(string productId)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Id, productId);
            return await _productCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetByShopIdAsync(string shopId)
        {
            var filter = Builders<Product>.Filter.And(
                Builders<Product>.Filter.Eq(p => p.ShopId, shopId),
                Builders<Product>.Filter.Eq(p => p.IsDeleted, false)
            );

            return await _productCollection.Find(filter).ToListAsync();
        }

        public async Task UpdateAsync(Product updatedProduct)
        {
            var existingProduct = await _productCollection
                .Find(p => p.Id == updatedProduct.Id)
                .FirstOrDefaultAsync();

            if (existingProduct == null) return;

            bool categoryChanged = !string.Equals(
                existingProduct.Category?.Trim(),
                updatedProduct.Category?.Trim(),
                StringComparison.OrdinalIgnoreCase
            );

            if (categoryChanged)
            {
                // Remove from old category
                var oldFilter = Builders<Category>.Filter
                    .Where(c => c.Name.ToLower() == existingProduct.Category.ToLower());
                var oldUpdate = Builders<Category>.Update
                    .Pull(c => c.ProductIds, existingProduct.Id)
                    .Inc(c => c.ProductCount, -1);
                await _categoryCollection.UpdateOneAsync(oldFilter, oldUpdate);

                // Add to new category
                var newCategoryName = updatedProduct.Category.Trim().ToLowerInvariant();
                var newCategory = await _categoryCollection
                    .Find(c => c.Name.ToLower() == newCategoryName)
                    .FirstOrDefaultAsync();

                if (newCategory == null)
                {
                    var createdCategory = new Category
                    {
                        Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(newCategoryName),
                        ProductCount = 1,
                        ProductIds = new List<string> { updatedProduct.Id },
                        CreatedAt = DateTime.UtcNow
                    };
                    await _categoryCollection.InsertOneAsync(createdCategory);
                }
                else
                {
                    var newUpdate = Builders<Category>.Update
                        .Push(c => c.ProductIds, updatedProduct.Id)
                        .Inc(c => c.ProductCount, 1);
                    await _categoryCollection.UpdateOneAsync(c => c.Id == newCategory.Id, newUpdate);
                }
            }

            var filter = Builders<Product>.Filter.Eq(p => p.Id, updatedProduct.Id);
            await _productCollection.ReplaceOneAsync(filter, updatedProduct);
        }

        public async Task DeleteAsync(string productId)
        {
            var product = await _productCollection.Find(p => p.Id == productId).FirstOrDefaultAsync();
            if (product == null) return;

            var filter = Builders<Product>.Filter.Eq(p => p.Id, productId);
            await _productCollection.DeleteOneAsync(filter);

            var categoryFilter = Builders<Category>.Filter
                .Where(c => c.Name.ToLower() == product.Category.ToLower());
            var update = Builders<Category>.Update
                .Pull(c => c.ProductIds, productId)
                .Inc(c => c.ProductCount, -1);

            await _categoryCollection.UpdateOneAsync(categoryFilter, update);
        }

        public async Task GenerateCategoriesFromExistingProducts()
        {
            var products = await _productCollection.Find(p => !p.IsDeleted).ToListAsync();

            var grouped = products
                .GroupBy(p => p.Category?.Trim()?.ToLower())
                .Where(g => !string.IsNullOrWhiteSpace(g.Key))
                .ToList();

            foreach (var group in grouped)
            {
                string categoryName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(group.Key);

                var exists = await _categoryCollection
                    .Find(c => c.Name.ToLower() == categoryName.ToLower())
                    .FirstOrDefaultAsync();

                if (exists != null) continue;

                var category = new Category
                {
                    Name = categoryName,
                    ProductCount = group.Count(),
                    ProductIds = group.Select(p => p.Id).ToList(),
                    CreatedAt = DateTime.UtcNow
                };

                await _categoryCollection.InsertOneAsync(category);
            }
        }

        public async Task<List<Product>> GetProductsByIdsAsync(List<string> ids)
        {
            var filter = Builders<Product>.Filter.In(p => p.Id, ids);
            return await _productCollection.Find(filter).ToListAsync();
        }


        public async Task<List<Product>> GetAllAsync()
        {
            return await _productCollection.Find(p => !p.IsDeleted).ToListAsync();
        }

    }
}
