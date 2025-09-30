using AutoMapper;
using Contracts.Events;
using MassTransit;
using MongoDB.Entities;
using SearchService.Entities;

namespace SearchService.Consumer;

public class AuctionDeletedConsumer : IConsumer<AuctionDeleted>
{
    public async Task Consume(ConsumeContext<AuctionDeleted> context)
    {
        Console.WriteLine($"Received AuctionCreated event for Auction ID: {context.Message.Id}");

        var result = await DB.DeleteAsync<Item>(context.Message.Id);

        if (!result.IsAcknowledged)
        {
            throw new Exception($"Failed to delete item with ID: {context.Message.Id}");
        }
    }
}
