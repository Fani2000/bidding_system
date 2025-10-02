using Contracts.Events;
using MassTransit;
using MongoDB.Entities;
using SearchService.Entities;

namespace SearchService.Consumer;

public class BidPlacedConsumer : IConsumer<BidPlaced>
{
    private readonly ILogger<BidPlacedConsumer> _logger;

    public BidPlacedConsumer(ILogger<BidPlacedConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<BidPlaced> context)
    {
        var message = context.Message;

        _logger.LogInformation("BidPlaced event received: {Message}", message);

        var auction = await DB.Find<Item>().OneAsync(message.AuctionId);

        if (
            auction == null
            || message.BidStatus.Contains("Accepted") && message.Amount > auction.CurrentHighBid
        )
        {
            if (auction != null)
            {
                auction.CurrentHighBid = message.Amount;

                await auction.SaveAsync();

                _logger.LogInformation(
                    "Auction with ID {AuctionId} updated with new highest bid: {BidAmount}",
                    message.AuctionId,
                    message.Amount
                );
            }
            else
            {
                _logger.LogWarning("Auction with ID {AuctionId} not found.", message.AuctionId);
            }
        }
        else
        {
            _logger.LogWarning("Auction with ID {AuctionId} not found.", message.AuctionId);
            return;
        }
    }
}
