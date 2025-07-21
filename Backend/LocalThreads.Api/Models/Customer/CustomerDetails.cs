using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace LocalThreads.Models.Customer
{
    public class CustomerRegistration
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string FullName { get; set; }
        public string PhoneNumber { get; set; }

        public string Email { get; set; } = null;
        public string Gender { get; set; } = null;
        public string AlternateMobile { get; set; } = null;
        public string Location { get; set; } = null;

        public DateTime? Dob { get; set; } = null;
        public string HintName { get; set; } = null;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLogin { get; set; } = null;

        public List<CartItem> Cart { get; set; } = new();
        public List<LikedProduct> LikedProducts { get; set; } = new();
    }

    public class CartItem
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime AddedAt { get; set; }
    }

    public class LikedProduct
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string ProductId { get; set; }
        public DateTime LikedAt { get; set; }
    }
}
