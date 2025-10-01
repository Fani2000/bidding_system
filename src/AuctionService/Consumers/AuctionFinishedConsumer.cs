using AuctionService.Data;
using AuctionService.Entities;
using BiddingSystem.Contracts.Events;
using MassTransit;

public class AuctionFinishedConsumer : IConsumer<AuctionFinished>
{
    private readonly AuctionDbContext _context;
    private readonly ILogger<AuctionFinishedConsumer> _logger;

    public AuctionFinishedConsumer(
        AuctionDbContext context,
        ILogger<AuctionFinishedConsumer> logger
    )
    {
        _context = context;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<AuctionFinished> context)
    {
        var message = context.Message;

        var auction = await _context.Auctions.FindAsync(message.AuctionId);

        if (context.Message.ItemSold)
        {
            auction.Winner = message.Winner;
            auction.SoldAmount = message.Amount;
        }

        auction.Status =
            auction.SoldAmount > auction.ReservePrice ? Status.Finished : Status.ReserveNotMet;

        await _context.SaveChangesAsync();

        _logger.LogInformation(
            "Auction with ID {AuctionId} has been marked as finished.",
            message.AuctionId
        );
    }
}
