using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Ploeh.Samples.BookingApi.UnitTests
{
    public class MaîtreDTests
    {
        [Fact]
        public void TryAcceptReturnsReservationIdInHappyPathScenario()
        {
            // properly initialize `reservation` here:
            Reservation reservation = null;
            // properly initialise `sut` here:
            MaîtreD sut = null;

            var actual = sut.TryAccept(reservation);

            // Uncomment and assign a proper value to `expected`:
            //Assert.Equal(expected, actual);
        }

        [Fact]
        public void TryAcceptOnInsufficientCapacity()
        {
            // properly initialize `reservation` here:
            Reservation reservation = null;
            // properly initialise `sut` here:
            MaîtreD sut = null;

            var actual = sut.TryAccept(reservation);

            Assert.Null(actual);
            // Add an assertion that sut.Repository.Create is NOT called here.
        }
    }
}
