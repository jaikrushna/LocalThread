using Microsoft.AspNetCore.Mvc;
using LocalThreads.Api.Services.Interfaces.Shared;
using MongoDB.Driver;
using LocalThreads.Models.Customer.Landing;

namespace LocalThreads.Api.Controllers.Customer
{
    [ApiController]
    [Route("api/[controller]")]
    public class LandingController : ControllerBase
    {
        private readonly IMongoDbService _mongo;
        private readonly IS3Service _s3;

        public LandingController(IMongoDbService mongo, IS3Service s3)
        {
            _mongo = mongo;
            _s3 = s3;
        }

        // POST: /api/landing/upload-banner
        [HttpPost("upload-banner")]
        public async Task<IActionResult> UploadBanner(IFormFile file, [FromForm] string redirectUrl)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Banner image file is required.");

            var imageUrl = await _s3.UploadFileAsync(file, "banners");

            var banner = new Banner
            {
                ImageUrl = imageUrl,
                RedirectUrl = redirectUrl,
                UploadedAt = DateTime.UtcNow
            };

            await _mongo.Banners.InsertOneAsync(banner);
            return Ok(banner);
        }



        // ✅ Kept: Get all banners
        [HttpGet("banners")]
        public async Task<IActionResult> GetBanners()
        {
            var banners = await _mongo.Banners
                .Find(_ => true)
                .SortByDescending(b => b.UploadedAt)
                .ToListAsync();

            return Ok(banners);
        }

        // ✅ Kept: Get all categories
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _mongo.Categories
                .Find(_ => true)
                .SortBy(c => c.Name)
                .ToListAsync();

            var response = categories.Select(c => new
            {
                c.Name,
                c.ProductCount
            });

            return Ok(response);
        }


        // ✅ Kept: Get popular products in a city
        [HttpGet("popular-products")]
        public async Task<IActionResult> GetPopularProducts([FromQuery] string city)
        {
            if (string.IsNullOrWhiteSpace(city))
                return BadRequest("City is required.");

            // 1. Get all approved shops in the given city
            var shops = await _mongo.Shops
                .Find(s => s.City.ToLower() == city.ToLower() && s.IsApproved)
                .ToListAsync();

            var shopIds = shops.Select(s => s.Id).ToList();
            var shopMap = shops.ToDictionary(s => s.Id, s => s.ShopName);

            // 2. Get in-stock, non-deleted products from these shops
            var products = await _mongo.Products
                .Find(p => shopIds.Contains(p.ShopId) && p.InStock && !p.IsDeleted)
                .SortByDescending(p => p.Rating) // 🔥 Popularity = High Rating
                .Limit(12)
                .ToListAsync();

            // 3. Prepare response
            var result = products.Select(p => new
            {
                p.Name,
                p.Price,
                p.ImageUrl,
                p.Category,
                p.Rating,
                p.Tag,
                ShopName = shopMap.GetValueOrDefault(p.ShopId, "Unknown")
            });

            return Ok(result);
        }


        // ✅ New: Get popular shops based on products in a city
        [HttpGet("popular-shops")]
        public async Task<IActionResult> GetPopularShops([FromQuery] string city)
        {
            if (string.IsNullOrWhiteSpace(city))
                return BadRequest("City is required.");

            // Step 1: Get approved shops in the city
            var shopList = await _mongo.Shops
                .Find(s => s.City.ToLower() == city.ToLower() && s.IsApproved)
                .ToListAsync();

            var shopIdMap = shopList.ToDictionary(s => s.Id, s => s);
            var shopIds = shopList.Select(s => s.Id).ToList();

            // Step 2: Get top products (sorted by rating) from those shops
            var topProducts = await _mongo.Products
                .Find(p => shopIds.Contains(p.ShopId) && p.InStock && !p.IsDeleted)
                .SortByDescending(p => p.Rating)
                .Limit(30) // fetch more for diversity
                .ToListAsync();

            // Step 3: Group by Shop and calculate average rating
            var shopWithRatings = topProducts
                .GroupBy(p => p.ShopId)
                .Select(g =>
                {
                    var shop = shopIdMap[g.Key];
                    var averageRating = Math.Round(g.Average(p => p.Rating), 1);

                    return new
                    {
                        shopId = shop.Id,
                        shopName = shop.ShopName,
                        shopImageUrl = shop.ShopImageUrl,
                        city = shop.City,
                        category = shop.Category,
                        businessDescription = shop.BusinessDescription,
                        rating = averageRating
                    };
                })
                .OrderByDescending(s => s.rating)
                .Take(8)
                .ToList();

            return Ok(shopWithRatings);
        }

    }
}
