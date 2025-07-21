using LocalThreads.Api.Models;
using LocalThreads.Models.Customer.Landing;

namespace LocalThreads.Api.Repositories.Interfaces.Customer
{
    public interface ICategoryRepository
    {
        Task<Category?> GetByNameAsync(string categoryName);
    }
}
