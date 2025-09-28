using System;

namespace SearchService.Contracts
{
    // Raised when a new auction is created in the Auction Service
    public class AuctionCreated
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime AuctionEnd { get; set; }
        public string Seller { get; set; } = string.Empty;
        public string? Winner { get; set; }
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

    // Raised when an auction is updated
    public class AuctionUpdated
    {
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public int Mileage { get; set; }
        public int Year { get; set; }
    }

    // Raised when an auction is deleted
    public class AuctionDeleted
    {
        public string Id { get; set; } = string.Empty;
    }

    // Raised when an auction is finished
    public class AuctionFinished
    {
        public bool ItemSold { get; set; }
        public string AuctionId { get; set; } = string.Empty;
        public string Winner { get; set; } = string.Empty;
        public string Seller { get; set; } = string.Empty;
        public int? Amount { get; set; }
    }

    // Raised when a bid is placed
    public class BidPlaced
    {
        public string Id { get; set; } = string.Empty;
        public string AuctionId { get; set; } = string.Empty;
        public string Bidder { get; set; } = string.Empty;
        public DateTime BidTime { get; set; }
        public int Amount { get; set; }
        public string BidStatus { get; set; } = string.Empty;
    }
}
