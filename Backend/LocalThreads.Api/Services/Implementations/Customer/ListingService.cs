using LocalThreads.Api.DTOs.Request.Customer;
using LocalThreads.Api.DTOs.Response.Customer;
using LocalThreads.Api.Models.Shopkeeper;
using LocalThreads.Api.Repositories.Interfaces;
using LocalThreads.Api.Repositories.Interfaces.Customer;
using LocalThreads.Api.Repositories.Interfaces.Shared;
using LocalThreads.Api.Services.Interfaces.Customer;

namespace LocalThreads.Api.Services.Implementations.Customer
{
    public class ListingService : IListingService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ListingService(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<PagedProductResponse> GetFilteredProductsAsync(ProductFilterRequest request)
        {
            var category = await _categoryRepository.GetByNameAsync(request.CategoryName);
            if (category == null || category.ProductIds == null || !category.ProductIds.Any())
            {
                return new PagedProductResponse
                {
                    Products = new List<ProductCardDto>(),
                    Pagination = new PaginationMetadata
                    {
                        CurrentPage = request.Page,
                        PageSize = request.PageSize,
                        TotalItems = 0,
                        TotalPages = 0
                    }
                };
            }

            var allProducts = await _productRepository.GetProductsByIdsAsync(category.ProductIds);

            // Filter
            var filtered = allProducts.Where(p => !p.IsDeleted);

            if (!string.IsNullOrEmpty(request.Location))
                filtered = filtered.Where(p => string.Equals(p.City, request.Location, StringComparison.OrdinalIgnoreCase));

            if (request.MinPrice.HasValue)
                filtered = filtered.Where(p => p.Price >= request.MinPrice.Value);

            if (request.MaxPrice.HasValue)
                filtered = filtered.Where(p => p.Price <= request.MaxPrice.Value);

            // Sort
            var sortBy = request.SortBy?.ToLower();
            var sortOrder = request.SortOrder?.ToLower() ?? "asc";

            filtered = (sortBy, sortOrder) switch
            {
                ("price", "asc") => filtered.OrderBy(p => p.Price),
                ("price", "desc") => filtered.OrderByDescending(p => p.Price),
                ("rating", "asc") => filtered.OrderBy(p => p.Rating),
                ("rating", "desc") => filtered.OrderByDescending(p => p.Rating),
                _ => filtered.OrderByDescending(p => p.Rating)
            };

            var totalItems = filtered.Count();
            var totalPages = request.PageSize > 0 ? (int)Math.Ceiling(totalItems / (double)request.PageSize) : 0;

            var paginated = request.PageSize > 0
                ? filtered.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).ToList()
                : filtered.ToList();

            var result = paginated.Select(p => new ProductCardDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Rating = p.Rating,
                ImageUrl = p.ImageUrl
            }).ToList();

            return new PagedProductResponse
            {
                Products = result,
                Pagination = new PaginationMetadata
                {
                    CurrentPage = request.Page,
                    PageSize = request.PageSize,
                    TotalItems = totalItems,
                    TotalPages = totalPages
                }
            };
        }
    }
}
