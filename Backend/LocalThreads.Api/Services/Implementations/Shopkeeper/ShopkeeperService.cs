using LocalThreads.Api.DTOs.Requests.Shopkeeper;
using LocalThreads.Api.DTOs.Response.Shopkeeper;
using LocalThreads.Api.Models.Shopkeeper;
using LocalThreads.Api.Repositories.Interfaces.Shopkeeper;
using LocalThreads.Api.Services.Implementations.Shared;
using LocalThreads.Api.Services.Interfaces.Shopkeeper;
using MongoDB.Bson;

namespace LocalThreads.Api.Services.Implementations.Shopkeeper
{
    public class ShopkeeperService : IShopkeeperService
    {
        private readonly IShopRepository _shopRepository;
        private readonly IUploadService _uploadService;
        private readonly FirebaseAuthService _firebaseAuthService;

        public ShopkeeperService(
            IShopRepository shopRepository,
            IUploadService uploadService,
            FirebaseAuthService firebaseAuthService)
        {
            _shopRepository = shopRepository;
            _uploadService = uploadService;
            _firebaseAuthService = firebaseAuthService;
        }

        public async Task<ShopRegisterResponse> RegisterShopAsync(ShopRegisterRequest request)
        {
            var firebaseToken = await _firebaseAuthService.VerifyIdTokenAsync(request.FirebaseIdToken);
            if (firebaseToken == null)
            {
                throw new UnauthorizedAccessException("Invalid or expired OTP token.");
            }

            // 2. (Optional) Use verified phone number from Firebase
            string verifiedPhone = firebaseToken?.Claims["phone_number"]?.ToString();
            string firebaseUid = firebaseToken?.Uid;

            var shopId = ObjectId.GenerateNewId().ToString();

            var (shopImageUrl, governmentIdUrl) = await _uploadService.UploadShopkeeperFilesAsync(
                shopId,
                request.ShopImageFile,
                request.GovernmentIdFile
            );

            var shop = new Shop
            {
                Id = shopId,
                ShopName = request.ShopName,
                OwnerName = request.OwnerName,
                PhoneNumber = verifiedPhone ?? request.PhoneNumber,
                FirebaseUid = firebaseUid,
                Address = request.Address,
                City = request.City,
                Pincode = request.Pincode,
                Category = request.Category,
                BusinessDescription = request.BusinessDescription,
                ShopImageUrl = shopImageUrl,
                GovernmentIdUrl = governmentIdUrl,
                IsApproved = false,
                CreatedAt = DateTime.UtcNow
            };

            await _shopRepository.CreateShopAsync(shop);

            return new ShopRegisterResponse
            {
                Id = shop.Id,
                Message = "Shopkeeper registered and images uploaded successfully.",
                IsPendingApproval = true
            };
        }
        // public async Task UpdateShopRatingAsync(string shopId)
        // {
        //     // Get all rated products from this shop
        //     var shopProducts = await _mongo.Products
        //         .Find(p => p.ShopId == shopId && p.Rating > 0)
        //         .ToListAsync();

        //     if (!shopProducts.Any()) return;

        //     var averageRating = shopProducts.Average(p => p.Rating);
        //     var ratingCount = shopProducts.Count;

        //     var update = Builders<Shop>.Update
        //         .Set(s => s.Rating, Math.Round(averageRating, 1))
        //         .Set(s => s.RatingCount, ratingCount);

        //     await _mongo.Shops.UpdateOneAsync(s => s.Id == shopId, update);
        // }
    }
}
