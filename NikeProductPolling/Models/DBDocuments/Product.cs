using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace NikeProductPolling.Models.DBDocuments
{
    public class Product
    {
        [BsonElement("_id")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public IEnumerable<string> PIds { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string BrandId { get; set; }
        public bool Active { get; set; }
        public string Gender { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public IEnumerable<string> ColourWayIds { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public IEnumerable<string> PriceIds { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string WebsiteId { get; set; }
    }
}
