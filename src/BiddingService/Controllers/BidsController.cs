using BiddingService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;

public record BidInputModel(string AuctionId, int Amount);

[ApiController]
[Route("api/[controller]")]
public class BidsController : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> PlaceBid([FromQuery] BidInputModel model)
    {
        var (acutionId, amount) = model;

        if (amount <= 0)
        {
            return BadRequest("Bid amount must be greater than zero");
        }

        var auction = await DB.Find<Auction>().OneAsync(acutionId);

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
            AuctionId = acutionId,
            Amount = amount,
            Bidder = User.Identity.Name,
            BidTime = DateTime.UtcNow
        };

        if (auction.AuctionEnd < DateTime.UtcNow)
        {
            bid.BidStatus = BidStatus.Finished;
        }
        else
        {
            var highBid = await DB.Find<Bid>()
                .Match(b => b.AuctionId == acutionId)
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

        return Ok(bid);
    }

    [HttpGet("{auctionId}")]
    public async Task<IActionResult> GetBidsForAuction(string auctionId)
    {
        var bids = await DB.Find<Bid>()
            .Match(b => b.AuctionId == auctionId)
            .Sort(b => b.Descending(b => b.BidTime))
            .ExecuteAsync();

        return Ok(bids);
    }
}
