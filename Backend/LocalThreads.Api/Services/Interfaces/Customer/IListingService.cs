using LocalThreads.Api.DTOs.Request.Customer;
using LocalThreads.Api.DTOs.Response.Customer;

namespace LocalThreads.Api.Services.Interfaces.Customer
{
    public interface IListingService
    {
        Task<PagedProductResponse> GetFilteredProductsAsync(ProductFilterRequest request);
        
    }
}
