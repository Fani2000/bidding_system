using System.Text.Json;
using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Entities;

namespace SearchService.Data;

public static class DbInitialiser
{
    public static async void InitDb(WebApplication app)
    {
        await DB.InitAsync(
            "SearchDb",
            MongoClientSettings.FromConnectionString(
                app.Configuration.GetConnectionString("MongoDbConnection")
            )
        );

        await DB.Index<Item>()
            .Key(x => x.Make, KeyType.Text)
            .Key(x => x.Model, KeyType.Text)
            .Key(x => x.Color, KeyType.Text)
            .CreateAsync();

        var count = await DB.CountAsync<Item>();
        // if (count == 0)
        // {
        //     Console.WriteLine("Seeding data...");

        //     var itemData = await File.ReadAllTextAsync("Data/auctions.json");

        //     var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        //     var items = JsonSerializer.Deserialize<List<Item>>(itemData, options);

        //     if (items != null && items.Count > 0)
        //     {
        //         await DB.SaveAsync(items);
        //     }
        // }
        using var scope = app.Services.CreateScope();
        var auctionClient = scope.ServiceProvider.GetRequiredService<AuctionSvcHttpClient>();
        var items = await auctionClient.GetItemsForSearchDb();

        Console.WriteLine($"Fetched {items.Count} items from Auction Service.");

        if (items.Count > 0)
        {
            await DB.SaveAsync(items);
            Console.WriteLine("Database seeding completed.");
        }
    }
}
