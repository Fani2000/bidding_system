
using AuctionService.Data;
using Contracts.Events;
using MassTransit;

public class BidPlacedConsumer : IConsumer<BidPlaced>
{
    private readonly AuctionDbContext _context;
    private readonly ILogger<BidPlacedConsumer> _logger;

    public BidPlacedConsumer(
        AuctionDbContext context,
        ILogger<BidPlacedConsumer> logger
    )
    {
        _context = context;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<BidPlaced> context)
    {
        var message = context.Message;

        var auction = await _context.Auctions.FindAsync(message.AuctionId);

        if (auction == null)
        {
            _logger.LogWarning("Auction with ID {AuctionId} not found.", message.AuctionId);
            return;
        }

        if (message.Amount > auction.CurrentHighBid)
        {
            auction.CurrentHighBid = message.Amount;
            auction.Winner = message.Bidder;

            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Auction with ID {AuctionId} has a new highest bid of {Amount} by bidder {Bidder}.",
                message.AuctionId,
                message.Amount,
                message.Bidder
            );
        }
        else
        {
            _logger.LogInformation(
                "Received bid of {Amount} for auction ID {AuctionId} is lower than the current highest bid of {HighestBid}.",
                message.Amount,
                message.AuctionId,
                auction.CurrentHighBid
            );
        }
    }
}