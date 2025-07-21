using Microsoft.AspNetCore.Http;

namespace LocalThreads.Api.DTOs.Requests.Shopkeeper
{
    public class ShopImageUploadRequest
    {
        public string ShopId { get; set; }
        public IFormFile ShopImageFile { get; set; }
        public IFormFile GovernmentIdFile { get; set; }
    }
}
