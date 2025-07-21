
using LocalThreads.Api.DTOs.Request.Shopkeeper;
using LocalThreads.Api.Models.Shopkeeper;

namespace LocalThreads.Api.Services.Interfaces.Shopkeeper
{
    public interface IProductService
    {
        Task<string> CreateProductAsync(CreateProductRequest request, Shop shop);
        Task UpdateProductAsync(string productId, UpdateProductRequest request, string shopId);
        // Task DeleteProductAsync(string productId, string shopId);
        Task DeleteProductAsync(string id, string productId);
    }

}