
namespace LocalThreads.Api.DTOs.Request.Shopkeeper;
public class UpdateProductRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string Category { get; set; }
    public string City { get; set; }
    public string Tag { get; set; }
    public IFormFile? Image { get; set; }  // optional
}
