using LocalThreads.Api.DTOs.Requests;
using LocalThreads.Api.DTOs.Requests.Shopkeeper;
using LocalThreads.Api.Repositories.Interfaces;
using LocalThreads.Api.Repositories.Interfaces.Shopkeeper;
using LocalThreads.Api.Services.Interfaces;
using LocalThreads.Api.Services.Interfaces.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LocalThreads.Api.Controllers.Shopkeeper
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly IS3Service _s3Service;
        private readonly IShopRepository _shopRepository;

        public UploadController(IS3Service s3Service, IShopRepository shopRepository)
        {
            _s3Service = s3Service;
            _shopRepository = shopRepository;
        }

        [HttpPost("shop-docs")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadShopDocuments([FromForm] ShopImageUploadRequest request)
        {
            if (string.IsNullOrEmpty(request.ShopId))
                return BadRequest("Shop ID is required");

            string folder = $"shops/{request.ShopId}";

            string shopImageUrl = "";
            string govtIdUrl = "";

            if (request.ShopImageFile != null)
            {
                shopImageUrl = await _s3Service.UploadFileAsync(request.ShopImageFile, $"{folder}/front");
            }

            if (request.GovernmentIdFile != null)
            {
                govtIdUrl = await _s3Service.UploadFileAsync(request.GovernmentIdFile, $"{folder}/documents");
            }

            await _shopRepository.UpdateShopImageUrlsAsync(request.ShopId, shopImageUrl, govtIdUrl);

            return Ok(new
            {
                message = "Images uploaded and MongoDB updated.",
                shopImageUrl,
                governmentIdUrl = govtIdUrl
            });
        }
    }
}
