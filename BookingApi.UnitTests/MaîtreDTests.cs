using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Ploeh.Samples.BookingApi.UnitTests
{
    public class MaîtreDTests
    {
        private class HappyPath : TheoryData<Reservation, int>
        {
            public HappyPath()
            {
                Add(new Reservation { Date = new DateTime(2018,  8, 30), Quantity =  4 }, 10);
                Add(new Reservation { Date = new DateTime(2019,  9, 29), Quantity = 10 }, 10);
                Add(new Reservation { Date = new DateTime(2020, 10, 28), Quantity = 20 }, 20);
                Add(new Reservation { Date = new DateTime(2021, 11, 27), Quantity =  1 }, 22);
            }
        }

        [Theory, ClassData(typeof(HappyPath))]
        public void CanAcceptReturnsReservationInHappyPathScenario(
            Reservation reservation,
            int capacity)
        {
            var sut = new MaîtreD(capacity);
            var actual = sut.CanAccept(new Reservation[0], reservation);
            Assert.True(actual);
        }

        [Theory]
        [InlineData(4, 10)]
        [InlineData(3, 3)]
        [InlineData(11, 14)]
        public void CanAcceptOnInsufficientCapacity(int quantity, int capacity)
        {
            var reservation = new Reservation
            {
                Date = new DateTime(2018, 8, 30),
                Quantity = quantity
            };
            var sut = new MaîtreD(capacity);

            var actual = sut.CanAccept(
                new[] { new Reservation { Quantity = 7 } },
                reservation);

            Assert.False(actual);
        }
    }
}
