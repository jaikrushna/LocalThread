using LocalThreads.Api.DTOs.Requests.Shopkeeper;
using LocalThreads.Api.DTOs.Response.Shopkeeper;
using System.Threading.Tasks;

namespace LocalThreads.Api.Services.Interfaces.Shopkeeper
{
    public interface IShopkeeperService
    {
        Task<ShopRegisterResponse> RegisterShopAsync(ShopRegisterRequest request);
    }
}
