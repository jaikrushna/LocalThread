
namespace LocalThreads.Api.DTOs.Response.Customer;
public class PaginationMetadata
{
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public long TotalItems { get; set; }
}
