using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Entities;

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
    public async Task<IActionResult> SearchItems(
        string? searchTerm,
        int pageNumber = 1,
        int pageSize = 4
    )
    {
        _logger.LogInformation("Search query received: {Query}", searchTerm);

        var query = DB.PagedSearch<Item>();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query.Match(Search.Full, searchTerm).SortByTextScore();
        }

        query.Sort(b => b.CreatedAt, Order.Ascending).PageNumber(pageNumber).PageSize(pageSize);

        var result = await query.ExecuteAsync();

        return Ok(
            new
            {
                Items = result.Results,
                pageCount = result.PageCount,
                totalCount = result.TotalCount
            }
        );
    }
}
