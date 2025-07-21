using LocalThreads.Api.Models.Shopkeeper;
using LocalThreads.Api.Services.Interfaces.Shared;
using LocalThreads.Api.Services.Interfaces.Shopkeeper;
using LocalThreads.Api.Utils;
using System.Net;
using LocalThreads.Api.DTOs.Request.Shopkeeper;
using LocalThreads.Models.Customer.Landing;
using LocalThreads.Api.Repositories.Interfaces.Shared;

namespace LocalThreads.Api.Services.Implementations.Shopkeeper
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IS3Service _s3Service;

        public ProductService(IProductRepository repository, IS3Service s3Service)
        {
            _repository = repository;
            _s3Service = s3Service;
        }

        public async Task<string> CreateProductAsync(CreateProductRequest request, Shop shop)
        {
            var product = new Product
            {
                ShopId = shop.Id,
                ShopName = shop.ShopName,
                PhoneNumber = shop.PhoneNumber,
                ShopAddress = shop.Address,
                City = shop.City,
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Category = request.Category,
                Sizes = request.Sizes.Split(',').ToList(),
                Color = request.Color,
                Tag = request.Tag.Split(',').ToList(),
                Rating = 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false,
                InStock = true
            };

            if (request.Image != null)
            {
                product.ImageUrl = await _s3Service.UploadFileAsync(request.Image, "products");
            }

            await _repository.CreateAsync(product);
            return product.Id;
        }

        public async Task UpdateProductAsync(string productId, UpdateProductRequest request, string shopId)
        {
            var product = await _repository.GetByIdAsync(productId);

            if (product == null || product.ShopId != shopId)
                throw new UnauthorizedAccessException("Product not found or unauthorized");

            // 🧹 Delete old image if new one is uploaded
            if (request.Image != null && !string.IsNullOrEmpty(product.ImageUrl))
            {
                var oldS3Key = WebUtility.UrlDecode(S3Helper.ExtractKeyFromUrl(product.ImageUrl));
                await _s3Service.DeleteFileAsync(oldS3Key);
                product.ImageUrl = await _s3Service.UploadFileAsync(request.Image, "products");
            }

            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;
            product.Category = request.Category;
            product.City = request.City;
            product.Tag = request.Tag.Split(',').ToList();
            product.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(product);
        }

        public async Task DeleteProductAsync(string productId, string shopId)
        {
            var product = await _repository.GetByIdAsync(productId);

            if (product == null || product.ShopId != shopId)
                throw new UnauthorizedAccessException("Product not found or unauthorized");

            // ❌ Delete image from S3 if exists
            if (!string.IsNullOrEmpty(product.ImageUrl))
            {
                var s3Key = WebUtility.UrlDecode(S3Helper.ExtractKeyFromUrl(product.ImageUrl));
                Console.WriteLine($"Deleting image from S3: {s3Key}");
                
                await _s3Service.DeleteFileAsync(s3Key);
            }

            await _repository.DeleteAsync(productId);
        }

        // 🔧 Helper to extract S3 key from full URL
    }
}
