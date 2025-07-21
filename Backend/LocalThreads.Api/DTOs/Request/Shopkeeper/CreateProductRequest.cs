
namespace LocalThreads.Api.DTOs.Request.Shopkeeper;

public class CreateProductRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string Category { get; set; } // From shopkeeper's shop location
    public string Tag { get; set; }  // Optional product tag: "New", "Trending"
    public string Sizes { get; set; } // CSV from form: "S,M,L"
    public string Color { get; set; }
    public IFormFile Image { get; set; }
}
