using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace LocalThreads.Api.Models.Shopkeeper
{
    public class Shop
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("shopName")]
        public string ShopName { get; set; }

        [BsonElement("ownerName")]
        public string OwnerName { get; set; }

        [BsonElement("phoneNumber")]
        public string PhoneNumber { get; set; }

        [BsonElement("address")]
        public string Address { get; set; }

        [BsonElement("city")]
        public string City { get; set; }

        [BsonElement("pincode")]
        public string Pincode { get; set; }

        [BsonElement("category")]
        public string Category { get; set; }

        [BsonElement("businessDescription")]
        public string BusinessDescription { get; set; }

        [BsonElement("shopImageUrl")]
        public string ShopImageUrl { get; set; } = "";

        [BsonElement("governmentIdUrl")]
        public string GovernmentIdUrl { get; set; } = "";

        [BsonElement("isApproved")]
        public bool IsApproved { get; set; } = false;

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string FirebaseUid { get; set; }

        // ✅ New: Average rating of products in this shop
        [BsonElement("rating")]
        public double Rating { get; set; } = 0;

        // ✅ New: Total number of ratings across products (optional)
        [BsonElement("ratingCount")]
        public int RatingCount { get; set; } = 0;

    }
}
