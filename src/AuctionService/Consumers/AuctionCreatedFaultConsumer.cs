using Contracts.Events;
using MassTransit;

namespace AuctionService.Consumers;

public class AuctionCreatedFaultConsumer : IConsumer<Fault<AuctionCreated>>
{
    public async Task Consume(ConsumeContext<Fault<AuctionCreated>> context)
    {
        Console.WriteLine(
            $"Fault received for AuctionCreated event. CorrelationId: {context.Message.Message.Model}, Exceptions: {string.Join(", ", context.Message.Exceptions.Select(e => e.Message))}"
        );
        var exception = context.Message.Exceptions.FirstOrDefault();

        if (exception.ExceptionType == "System.ArgumentException")
        {
            context.Message.Message.Model = "FooBar";

            await context.Publish(context.Message.Message);
        }
        else
        {
            // General fault handling
            Console.WriteLine("Handling general fault.");
        }
    }
}
