using LocalThreads.Api.Models;
using LocalThreads.Models.Customer.Landing;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocalThreads.Api.Repositories.Interfaces.Shared
{
    public interface IProductRepository
    {
        Task<string> CreateAsync(Product product);
        Task<Product> GetByIdAsync(string productId);
        Task<IEnumerable<Product>> GetByShopIdAsync(string shopId);
        Task UpdateAsync(Product product);
        Task DeleteAsync(string productId);

        Task GenerateCategoriesFromExistingProducts();

        Task<List<Product>> GetProductsByIdsAsync(List<string> ids);
        Task<List<Product>> GetAllAsync(); // Optional if needed for broader queries



    }
}
