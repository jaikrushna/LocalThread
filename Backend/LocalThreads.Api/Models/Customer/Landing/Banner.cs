using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace LocalThreads.Models.Customer.Landing
{
    public class Banner
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("imageUrl")]
        public string ImageUrl { get; set; }

        [BsonElement("redirectUrl")]
        public string RedirectUrl { get; set; }

        [BsonElement("uploadedAt")]
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}