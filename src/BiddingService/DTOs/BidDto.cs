public class BidDto
{
    public string Id { get; set; }
    public string AuctionId { get; set; } = string.Empty;
    public int Amount { get; set; }
    public string Bidder { get; set; } = string.Empty; // from JWT claims
    public DateTime BidTime { get; set; }
    public string BidStatus { get; set; } = string.Empty;
}
