using FirebaseAdmin.Auth;
using LocalThreads.Api.DTOs.Request.Shopkeeper;
using LocalThreads.Api.DTOs.Requests.Shopkeeper;
using LocalThreads.Api.Models.Shopkeeper;
using LocalThreads.Api.Repositories.Interfaces.Shopkeeper;
using LocalThreads.Api.Services.Interfaces.Shopkeeper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LocalThreads.Api.Controllers.Shopkeeper
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IShopRepository _shopRepository;

        public ProductsController(IProductService productService, IShopRepository shopRepository)
        {
            _productService = productService;
            _shopRepository = shopRepository;
        }

        private async Task<Shop> GetShopFromFirebaseAsync()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);
            var firebaseUid = decodedToken.Uid;

            var shop = await _shopRepository.GetByFirebaseUidAsync(firebaseUid);
            return shop;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateProductRequest request)
        {
            var shop = await GetShopFromFirebaseAsync();
            if (shop == null)
                return Unauthorized("Shop not found");

            var productId = await _productService.CreateProductAsync(request, shop);
            return Ok(new { productId, message = "Product uploaded successfully" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromForm] UpdateProductRequest request)
        {
            var shop = await GetShopFromFirebaseAsync();
            if (shop == null)
                return Unauthorized("Shop not found");

            await _productService.UpdateProductAsync(id, request, shop.Id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var shop = await GetShopFromFirebaseAsync();
            if (shop == null)
                return Unauthorized("Shop not found");

            await _productService.DeleteProductAsync(id, shop.Id);
            return NoContent();
        }
    }
}
