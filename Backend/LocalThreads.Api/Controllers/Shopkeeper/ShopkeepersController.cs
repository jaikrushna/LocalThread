using LocalThreads.Api.DTOs.Requests.Shopkeeper;
using LocalThreads.Api.Services.Interfaces.Shopkeeper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LocalThreads.Api.Controllers.Shopkeeper
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShopkeepersController : ControllerBase
    {
        private readonly IShopkeeperService _shopkeeperService;
        
        public ShopkeepersController(IShopkeeperService shopkeeperService)
        {
            _shopkeeperService = shopkeeperService;
        }
        

        [HttpPost("register")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> RegisterShopkeeper([FromForm] ShopRegisterRequest request)
        {
            var result = await _shopkeeperService.RegisterShopAsync(request);
            return Ok(result);
        }
    }
}
