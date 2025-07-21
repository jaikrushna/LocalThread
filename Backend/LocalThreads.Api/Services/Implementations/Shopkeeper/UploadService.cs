using LocalThreads.Api.Services.Interfaces.Shared;
using LocalThreads.Api.Services.Interfaces.Shopkeeper;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace LocalThreads.Api.Services.Implementations.Shopkeeper
{
    public class UploadService : IUploadService
    {
        private readonly IS3Service _s3Service;

        public UploadService(IS3Service s3Service)
        {
            _s3Service = s3Service;
        }

        public async Task<(string ShopImageUrl, string GovernmentIdUrl)> UploadShopkeeperFilesAsync(string shopId, IFormFile shopImage, IFormFile governmentId)
        {
            string basePath = $"shops/{shopId}";

            string shopImageUrl = "";
            string governmentIdUrl = "";

            if (shopImage != null)
            {
                shopImageUrl = await _s3Service.UploadFileAsync(shopImage, $"{basePath}/front");
            }

            if (governmentId != null)
            {
                governmentIdUrl = await _s3Service.UploadFileAsync(governmentId, $"{basePath}/documents");
            }

            return (shopImageUrl, governmentIdUrl);
        }
    }
}
