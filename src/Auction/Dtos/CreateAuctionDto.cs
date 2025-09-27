namespace Auction.Dtos;

public class CreateAuctionDto
{
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public int Mileage { get; set; }
    public int Year { get; set; }
    public int ReservePrice { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public DateTime AuctionEnd { get; set; }
}
