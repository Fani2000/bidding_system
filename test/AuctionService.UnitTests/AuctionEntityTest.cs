using AuctionService.Entities;

namespace AuctionService.UnitTests;

// How I name tests Method_Scenario_ExpectedResults

public class AuctionEntityTest
{
    public class AuctionEntityTests
    {
        [Fact]
        public void HasReservePrice_ReservePriceGtZero_True()
        {
            // arrange
            var auction = new Auction { Id = Guid.NewGuid(), ReservePrice = 10 };

            // act
            var result = auction.HasReservePrice();

            // assert
            Assert.True(result);
        }

        [Fact]
        public void HasReservePrice_ReservePriceIsZero_False()
        {
            // arrange
            var auction = new Auction { Id = Guid.NewGuid(), ReservePrice = 0 };

            // act
            var result = auction.HasReservePrice();

            // assert
            Assert.False(result);
        }
    }
}
