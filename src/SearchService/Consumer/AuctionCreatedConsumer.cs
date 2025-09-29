using AutoMapper;
using Contracts.Events;
using MassTransit;
using MongoDB.Entities;

namespace SearchService.Consumer;

public class AuctionCreatedConsumer : IConsumer<AuctionCreated>
{
    private readonly AutoMapper.IMapper _mapper;

    public AuctionCreatedConsumer(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<AuctionCreated> context)
    {
        Console.WriteLine($"Received AuctionCreated event for Auction ID: {context.Message.Id}");

        var item = _mapper.Map<Entities.Item>(context.Message);

        await item.SaveAsync();
    }
}
