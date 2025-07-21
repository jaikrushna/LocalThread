using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace LocalThreads.Api.Services.Interfaces.Shopkeeper
{
    public interface IUploadService
    {
        Task<(string ShopImageUrl, string GovernmentIdUrl)> UploadShopkeeperFilesAsync(string shopId, IFormFile shopImage, IFormFile governmentId);
    }
}
