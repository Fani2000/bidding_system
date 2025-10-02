using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MongoDB.Driver;
using MongoDB.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<AuctionCreatedConsumer>();

    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("bids", false));

    x.UsingRabbitMq(
        (context, cfg) =>
        {
            cfg.Host(
                builder.Configuration["RabbitMq:Host"],
                "/",
                h =>
                {
                    h.Username(builder.Configuration.GetValue("RabbitMq:Username", "guest"));
                    h.Password(builder.Configuration.GetValue("RabbitMq:Password", "guest"));
                }
            );
            cfg.ConfigureEndpoints(context);
        }
    );
});

builder
    .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["IdentityServiceUrl"];
        options.Audience = "auctionApp";
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters.ValidateAudience = false;
        options.TokenValidationParameters.NameClaimType = "username";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await DB.InitAsync(
    "BidDb",
    MongoClientSettings.FromConnectionString(
        builder.Configuration.GetConnectionString("BidDbConnection")!
    )
);

app.Run();
