using AutoMapper;
using BiddingService.Models;
using Contracts.Events;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;

public record BidInputModel(string AuctionId, int Amount);

[ApiController]
[Route("api/[controller]")]
public class BidsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publisher;

    public BidsController(IMapper mapper, IPublishEndpoint publisher)
    {
        _mapper = mapper;
        _publisher = publisher;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> PlaceBid([FromQuery] BidInputModel model)
    {
        var (auctionId, amount) = model;

        if (amount <= 0)
        {
            return BadRequest("Bid amount must be greater than zero");
        }

        var auction = await DB.Find<Auction>().OneAsync(auctionId);

        if (auction == null)
        {
            return NotFound("Auction not found");
        }

        if (auction.Seller == User.Identity.Name)
        {
            return BadRequest("You cannot bid on your own auction");
        }

        var bid = new Bid
        {
            AuctionId = auctionId,
            Amount = amount,
            Bidder = User.Identity.Name,
        };

        if (auction.AuctionEnd < DateTime.UtcNow)
        {
            bid.BidStatus = BidStatus.Finished;
        }
        else
        {
            var highBid = await DB.Find<Bid>()
                .Match(b => b.AuctionId == auctionId)
                .Sort(b => b.Descending(b => b.Amount))
                .ExecuteFirstAsync();

            if (highBid != null && highBid.Amount >= amount)
            {
                bid.BidStatus =
                    amount > auction.ReservePrice
                        ? BidStatus.Accepted
                        : BidStatus.AcceptedBelowReserve;
            }

            if (highBid != null && bid.Amount <= highBid.Amount)
            {
                bid.BidStatus = BidStatus.TooLow;
            }
        }

        await bid.SaveAsync();

        await _publisher.Publish(_mapper.Map<BidPlaced>(bid));

        return Ok(_mapper.Map<BidDto>(bid));
    }

    [HttpGet("{auctionId}")]
    public async Task<ActionResult<List<BidDto>>> GetBidsForAuction(string auctionId)
    {
        var bids = await DB.Find<Bid>()
            .Match(b => b.AuctionId == auctionId)
            .Sort(b => b.Descending(b => b.BidTime))
            .ExecuteAsync();

        var results = bids.Select(b => _mapper.Map<BidDto>(b)).ToList();

        return Ok(results);
    }
}
