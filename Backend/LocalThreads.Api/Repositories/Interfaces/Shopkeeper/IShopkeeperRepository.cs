using LocalThreads.Api.Models.Shopkeeper;
using LocalThreads.Api.DTOs.Requests.Shopkeeper;  
using System.Threading.Tasks;

namespace LocalThreads.Api.Repositories.Interfaces.Shopkeeper
{
    public interface IShopRepository
    {
        Task<Shop> GetByFirebaseUidAsync(string firebaseUid);
        Task<Shop> CreateShopAsync(Shop shop);
        Task<Shop> GetByPhoneNumberAsync(string phoneNumber);
        Task<Shop> GetByIdAsync(string shopId);
        Task UpdateShopImageUrlsAsync(string shopId, string shopImageUrl, string governmentIdUrl);
    }
}
