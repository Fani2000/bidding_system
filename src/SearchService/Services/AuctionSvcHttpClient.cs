using MongoDB.Entities;
using SearchService.Entities;

public class AuctionSvcHttpClient(HttpClient httpClient, IConfiguration configuration)
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly IConfiguration _configuration = configuration;

    public async Task<List<Item>> GetItemsForSearchDb()
    {
        var lastUpdated = await DB.Find<Item, string>()
            .Sort(i => i.UpdatedAt, Order.Descending)
            .Project(i => i.UpdatedAt.ToString("o"))
            .ExecuteAnyAsync();

        string queryParam = lastUpdated != default ? lastUpdated.ToString() : string.Empty;

        var url = $"{_configuration["AuctionServiceUrl"]}/api/auctions";

        if (!string.IsNullOrEmpty(queryParam))
        {
            url += $"?date={Uri.EscapeDataString(queryParam)}";
        }

        var items = await _httpClient.GetFromJsonAsync<List<Item>>(url);

        return items ?? new List<Item>();
    }
}
