using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace NikeProductPolling.Models.DBDocuments
{
    public class Price
    {
        [BsonElement("_id")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string ProductId { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string ColourwayId { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string BrandId { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string WebsiteId { get; set; }
        public string Currency { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal FullPrice { get; set; }
        public bool Discounted { get; set; }
        public bool Active { get; set; }
    }
}
