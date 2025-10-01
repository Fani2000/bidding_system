using BiddingSystem.Contracts.Events;
using MassTransit;
using MongoDB.Entities;
using SearchService.Entities;

public class AuctionFinishedConsumer : IConsumer<AuctionFinished>
{
    private readonly ILogger<AuctionFinishedConsumer> _logger;

    public AuctionFinishedConsumer(ILogger<AuctionFinishedConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<AuctionFinished> context)
    {
        var message = context.Message;

        var auction = await DB.Find<Item>().OneAsync(message.AuctionId);

        if (message.ItemSold)
        {
            auction.Winner = message.Winner;
            auction.SoldAmount = (int)message.Amount;
        }

        auction.Status = "Finished";

        await auction.SaveAsync();
    }
}
