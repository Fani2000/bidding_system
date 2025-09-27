namespace Auction.Entities;

public class Item
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Make { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    public int Year { get; set; }

    public string Color { get; set; } = string.Empty;

    public int Mileage { get; set; }

    public string ImageUrl { get; set; } = string.Empty;

    // Navigation back to Auction
    public Auction Auction { get; set; } = null!;

    public Guid AuctionId { get; set; }
}
