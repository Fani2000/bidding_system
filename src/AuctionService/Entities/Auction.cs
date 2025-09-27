namespace AuctionService.Entities;

[Table("Auctions")]
public class Auction
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public int ReservePrice { get; set; } = 0;

    // Seller username from claims
    public string Seller { get; set; } = string.Empty;

    // Nullable winner username
    public string? Winner { get; set; }

    public int? SoldAmount { get; set; }

    public int? CurrentHighBid { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public DateTime AuctionEnd { get; set; }

    public Status Status { get; set; } = Status.Live;

    // Navigation property
    public Item Item { get; set; } = null!;

    public Guid ItemId { get; set; }
}
