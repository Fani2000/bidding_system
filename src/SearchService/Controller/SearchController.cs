using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Entities;
using SearchService.RequestHelpers;

namespace SearchService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SearchController : ControllerBase
{
    private readonly ILogger<SearchController> _logger;

    public SearchController(ILogger<SearchController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> SearchItems([FromQuery] SearchParams searchParams)
    {
        var searchTerm = searchParams.searchTerm;
        var pageNumber = searchParams.pageNumber;
        var pageSize = searchParams.pageSize;

        _logger.LogInformation("Search query received: {Query}", searchTerm);

        // Start a paged search on the Item collection
        var query = DB.PagedSearch<Item>();

        // Full-text search if a term was provided
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query.Match(Search.Full, searchTerm).SortByTextScore(); // text score sorting only if full-text search used
        }

        // Filtering by auction status
        switch (searchParams.filterBy?.ToLowerInvariant())
        {
            case "finished":
                query.Match(i => i.AuctionEnd < DateTime.UtcNow);
                break;
            case "endingsoon":
                query.Match(i =>
                    i.AuctionEnd < DateTime.UtcNow.AddHours(6) && i.AuctionEnd > DateTime.UtcNow
                );
                break;
            default:
                query.Match(i => i.AuctionEnd > DateTime.UtcNow);
                break;
        }

        // Sorting
        switch (searchParams.orderBy?.ToLowerInvariant())
        {
            case "make":
                query.Sort(i => i.Make, Order.Ascending);
                break;
            case "new":
                query.Sort(i => i.CreatedAt, Order.Descending); // newest first
                break;
            default:
                query.Sort(i => i.AuctionEnd, Order.Ascending);
                break;
        }

        // Paging
        query.PageNumber(pageNumber).PageSize(pageSize);

        var result = await query.ExecuteAsync();

        return Ok(
            new
            {
                Items = result.Results,
                result.PageCount,
                result.TotalCount
            }
        );
    }
}
