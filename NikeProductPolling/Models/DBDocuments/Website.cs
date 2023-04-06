using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace NikeProductPolling.Models.DBDocuments
{
    public class Website
    {
        [BsonElement("_id")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string CountryId { get; set; }
        public string Url { get; set; }
        public bool Active { get; set; }
    }
}
