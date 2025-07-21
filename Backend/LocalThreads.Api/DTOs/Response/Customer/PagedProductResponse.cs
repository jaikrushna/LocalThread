
namespace LocalThreads.Api.DTOs.Response.Customer;
public class PagedProductResponse
{
    public List<ProductCardDto> Products { get; set; }
    public PaginationMetadata Pagination { get; set; }
}
