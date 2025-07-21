using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace LocalThreads.Api.Models.Shared
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string CustomerId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string ShopId { get; set; }

        public string ShopName { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public string Status { get; set; }  // e.g., Pending, Approved, Delivered, Cancelled

        public List<OrderItem> OrderItems { get; set; }

        public DeliveryAddress DeliveryAddress { get; set; }
    }

    public class OrderItem
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string ProductId { get; set; }

        public string ProductName { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }

    public class DeliveryAddress
    {
        public string Name { get; set; }

        public string Mobile { get; set; }

        public string AddressLine { get; set; }

        public string City { get; set; }

        public string Pincode { get; set; }
    }
}
