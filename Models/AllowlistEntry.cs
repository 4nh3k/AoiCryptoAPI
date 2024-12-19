using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AoiCryptoAPI.Models
{
    public class AllowlistEntry
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string PoolAddress { get; set; }

        public string UserAddress { get; set; }

        public string EmailAddress { get; set; }

        public string UserFullName { get; set; }

        public string Status { get; set; }
    }
}
