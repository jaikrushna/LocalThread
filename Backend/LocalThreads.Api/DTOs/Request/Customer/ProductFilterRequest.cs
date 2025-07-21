namespace LocalThreads.Api.DTOs.Request.Customer
{
    public class ProductFilterRequest
    {
        public string CategoryName { get; set; } = string.Empty;
        public string? Location { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? SortBy { get; set; } // price, rating, views
        public string? SortOrder { get; set; } // asc or desc
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
