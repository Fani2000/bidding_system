using System;

namespace BiddingService.Events
{
    // Emitted when a bid is placed
    public class BidPlaced
    {
        public string Id { get; set; } = string.Empty;
        public string AuctionId { get; set; } = string.Empty;
        public string Bidder { get; set; } = string.Empty;
        public DateTime BidTime { get; set; }
        public int Amount { get; set; }
        public string BidStatus { get; set; } = string.Empty;
    }

    // Emitted when an auction finishes
    public class AuctionFinished
    {
        public bool ItemSold { get; set; }
        public string AuctionId { get; set; } = string.Empty;
        public string Winner { get; set; } = string.Empty;
        public string Seller { get; set; } = string.Empty;
        public int Amount { get; set; }
    }

    // Consumed when an auction is created in Auction Service
    public class AuctionCreated
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime AuctionEnd { get; set; }
        public string Seller { get; set; } = string.Empty;
        public string Winner { get; set; } = string.Empty;
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public string Color { get; set; } = string.Empty;
        public int Mileage { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int ReservePrice { get; set; }
        public int? SoldAmount { get; set; }
        public int? CurrentHighBid { get; set; }
    }
}
