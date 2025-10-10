using MassTransit;
using NotificationService.Consumers;
using NotificationService.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddCors();

// MassTransit + RabbitMQ
builder.Services.AddMassTransit(x =>
{
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("nt", false));

    x.AddConsumersFromNamespaceContaining<AuctionCreatedConsumer>();

    x.UsingRabbitMq(
        (context, cfg) =>
        {
            cfg.Host(
                builder.Configuration["RabbitMq:Host"],
                "/",
                h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                }
            );

            cfg.ConfigureEndpoints(context);
        }
    );
});

var app = builder.Build();

app.UseCors();

app.MapHub<NotificationHub>("/notifications"); // SignalR Hub endpoint

app.Run();
