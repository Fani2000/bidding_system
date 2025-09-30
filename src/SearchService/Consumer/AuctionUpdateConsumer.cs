using AutoMapper;
using Contracts.Events;
using MassTransit;
using MongoDB.Entities;
using SearchService.Entities;

public class AuctionUpdateConsumer : IConsumer<AuctionUpdated>
{
    private readonly ILogger<AuctionUpdateConsumer> _logger;
    private readonly IMapper _mapper;

    public AuctionUpdateConsumer(ILogger<AuctionUpdateConsumer> logger, IMapper mapper = null)
    {
        _logger = logger;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<AuctionUpdated> context)
    {
        var item = _mapper.Map<Item>(context.Message);

        _logger.LogInformation(
            "Received AuctionUpdated event for Auction ID: {AuctionId}",
            item.ID
        );

        var result = await DB.Update<Item>()
            .Match(i => i.ID == item.ID)
            .ModifyOnly(
                x => new
                {
                    x.Make,
                    x.Model,
                    x.Year,
                    x.Mileage,
                    x.Color,
                    x.UpdatedAt
                },
                item
            )
            .ExecuteAsync();

        if (!result.IsAcknowledged)
        {
            _logger.LogError("Failed to update item with ID: {ItemId} in MongoDB", item.ID);
            throw new Exception($"Failed to update item with ID: {item.ID}");
        }
    }
}
