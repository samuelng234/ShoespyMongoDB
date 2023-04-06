using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace NikeProductPolling.Models.DBDocuments
{
    public class ColourWay
    {
        [BsonElement("_id")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string ProductId { get; set; }
        public string PId { get; set; }
        public string Sku { get; set; }
        public string Guid { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public bool InStock { get; set; }
        public bool IsComingSoon { get; set; }
        public bool IsNew { get; set; }
        public bool Active { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string PriceId { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string WebsiteId { get; set; }
        public IEnumerable<string> Images { get; set; }
    }
}
