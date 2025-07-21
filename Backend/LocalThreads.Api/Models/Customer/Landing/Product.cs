using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace LocalThreads.Models.Customer.Landing
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }  // MongoDB's unique _id

        [BsonElement("shopId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ShopId { get; set; }  // Reference to the shopkeeper

        [BsonElement("shopname")]
        public string ShopName { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("price")]
        public decimal Price { get; set; }

        [BsonElement("imageUrl")]
        public string ImageUrl { get; set; }


        [BsonElement("sizes")]
        [BsonIgnoreIfNull]
        public List<string> Sizes { get; set; }

        [BsonElement("color")]
        [BsonIgnoreIfNull]
        public string Color { get; set; }

        [BsonElement("category")]
        public string Category { get; set; }

        [BsonElement("rating")]
        [BsonIgnoreIfNull]
        public double Rating { get; set; } = 0;

        [BsonElement("inStock")]
        public bool InStock { get; set; }

        [BsonElement("isDeleted")]
        [BsonIgnoreIfNull]
        public bool IsDeleted { get; set; } = false;

        [BsonElement("city")]
        public string City { get; set; }

        [BsonElement("phonenumber")]
        public string PhoneNumber { get; set; }

        [BsonElement("shopAddress")]
        public string ShopAddress { get; set; }

        [BsonElement("tag")]
        [BsonIgnoreIfNull]
        public List<string> Tag { get; set; } // e.g., "New", "Sale"

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
