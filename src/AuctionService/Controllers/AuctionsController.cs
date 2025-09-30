using AuctionService.Data;
using AuctionService.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Contracts.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers;

[ApiController]
[Route("api/auctions")]
public class AuctionsController(
    ILogger<AuctionsController> logger,
    AuctionDbContext context,
    IMapper mapper,
    IPublishEndpoint publishEndpoint
) : ControllerBase
{
    private readonly ILogger<AuctionsController> _logger = logger;
    private readonly AuctionDbContext _context = context;
    private readonly IMapper _mapper = mapper;
    private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AuctionDto>>> GetAuctions(string? date)
    {
        var query = _context.Auctions.OrderBy(x => x.Item.Make).AsQueryable();

        if (!string.IsNullOrEmpty(date))
        {
            if (DateTime.TryParse(date, out var parsedDate))
            {
                query = query.Where(a => a.UpdatedAt.CompareTo(parsedDate.ToUniversalTime()) > 0);
            }
            else
            {
                return BadRequest("Invalid date format. Please use a valid date.");
            }
        }

        return Ok(await query.ProjectTo<AuctionDto>(_mapper.ConfigurationProvider).ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid id)
    {
        var auction = await _context
            .Auctions.Include(a => a.Item)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (auction == null)
            return NotFound();

        return Ok(_mapper.Map<AuctionDto>(auction));
    }

    [HttpPost]
    public async Task<ActionResult<AuctionDto>> CreateAuction(CreateAuctionDto createAuctionDto)
    {
        var auction = _mapper.Map<Entities.Auction>(createAuctionDto);
        auction.Id = Guid.NewGuid();
        auction.CreatedAt = DateTime.UtcNow;

        var auctionDto = _mapper.Map<AuctionDto>(auction);

        var auctionCreatedEvent = _mapper.Map<AuctionCreated>(auction);

        await _publishEndpoint.Publish(auctionCreatedEvent);

        _context.Auctions.Add(auction);

        var result = await _context.SaveChangesAsync() > 0;

        if (!result)
        {
            _logger.LogError("Problem saving new auction to database");
            return StatusCode(500, "A problem happened while handling your request.");
        }

        return CreatedAtAction(
            nameof(GetAuctionById),
            new { id = auction.Id },
            auctionCreatedEvent
        );
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<AuctionDto>> UpdateAuction(
        Guid id,
        UpdateAuctionDto updateAuctionDto
    )
    {
        var auction = await _context
            .Auctions.Include(a => a.Item)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (auction == null)
            return NotFound();

        auction.Item.Make = updateAuctionDto.Make ?? auction.Item.Make;
        auction.Item.Model = updateAuctionDto.Model ?? auction.Item.Model;
        auction.Item.Color = updateAuctionDto.Color ?? auction.Item.Color;
        auction.Item.Mileage = updateAuctionDto.Mileage ?? auction.Item.Mileage;
        auction.Item.Year = updateAuctionDto.Year ?? auction.Item.Year;

        await _publishEndpoint.Publish(_mapper.Map<AuctionUpdated>(auction));

        var result = await _context.SaveChangesAsync() > 0;

        if (!result)
        {
            _logger.LogError("Problem updating auction in database");
            return StatusCode(500, "A problem happened while handling your request.");
        }

        var auctionDto = _mapper.Map<AuctionDto>(auction);

        return Ok(auctionDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAuction(Guid id)
    {
        var auction = await _context.Auctions.FindAsync(id);

        if (auction == null)
            return NotFound();

        await _publishEndpoint.Publish(new AuctionDeleted { Id = id.ToString() });

        _context.Auctions.Remove(auction);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
