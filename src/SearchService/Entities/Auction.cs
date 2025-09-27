using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BiddingService.Entities
{
    public class Auction
    {
        [BsonId] // MongoDB ObjectId as string
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        public DateTime AuctionEnd { get; set; }

        public string Seller { get; set; } = string.Empty;

        public int ReservePrice { get; set; }

        public bool Finished { get; set; } = false;
    }
}
