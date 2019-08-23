using FsCheck;
using FsCheck.Xunit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Ploeh.Samples.BookingApi.UnitTests
{
    public class MaîtreDTests
    {
        private class HappyPath : TheoryData<IEnumerable<Reservation>, Reservation, int>
        {
            public HappyPath()
            {
                Add(new Reservation[0], new Reservation { Date = new DateTime(2018,  8, 30), Quantity =  4 }, 10);
                Add(new Reservation[0], new Reservation { Date = new DateTime(2019,  9, 29), Quantity = 10 }, 10);
                Add(new Reservation[0], new Reservation { Date = new DateTime(2020, 10, 28), Quantity = 20 }, 20);
                Add(new Reservation[0], new Reservation { Date = new DateTime(2021, 11, 27), Quantity =  1 }, 22);
                Add(new[] { new Reservation { Quantity = 2 } }, new Reservation { Quantity = 1 }, 3);
            }
        }

        [Theory, ClassData(typeof(HappyPath))]
        public void CanAcceptReturnsReservationInHappyPathScenario(
            IEnumerable<Reservation> reservations,
            Reservation reservation,
            int capacity)
        {
            var sut = new MaîtreD(capacity);
            var actual = sut.CanAccept(reservations, reservation);
            Assert.True(actual);
        }

        [Property]
        public void CanAcceptOnInsufficientCapacity(
            DateTime date,
            PositiveInt excessQuantity,
            NonNegativeInt capacitySurplus,
            NonNegativeInt reservedSeats)
        {
            var quantity = capacitySurplus.Item + excessQuantity.Item;
            var capacity = capacitySurplus.Item + reservedSeats.Item;
            var reservation = new Reservation
            {
                Date = date,
                Quantity = quantity
            };
            var sut = new MaîtreD(capacity);

            var actual = sut.CanAccept(
                new[] { new Reservation { Quantity = reservedSeats.Item } },
                reservation);

            Assert.False(actual);
        }
    }
}
