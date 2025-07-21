using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace LocalThreads.Models.Shopkeeper
{
    public enum OrderStatus
    {
        Pending,
        Approved,
        Rejected,
        Delivered
    }

    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string CustomerId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string ShopkeeperId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string ProductId { get; set; }

        public int Quantity { get; set; }

        [BsonRepresentation(BsonType.String)]
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;

        public string CustomerAddress { get; set; }

        public DateTime RequestedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
