using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BiddingService.Entities
{
    public class Bid
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonRepresentation(BsonType.ObjectId)]
        public string AuctionId { get; set; } = string.Empty;

        public DateTime BidTime { get; set; } = DateTime.UtcNow;

        public int Amount { get; set; }

        public BidStatus BidStatus { get; set; }
    }
}
