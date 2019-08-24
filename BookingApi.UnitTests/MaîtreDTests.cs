using FsCheck;
using FsCheck.Xunit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                Add(new Reservation[0], new Reservation { Date = new DateTime(2018,  8, 30), Quantity =  4 },  6);
                Add(new Reservation[0], new Reservation { Date = new DateTime(2019,  9, 29), Quantity = 10 },  0);
                Add(new Reservation[0], new Reservation { Date = new DateTime(2020, 10, 28), Quantity = 20 },  0);
                Add(new Reservation[0], new Reservation { Date = new DateTime(2021, 11, 27), Quantity =  1 }, 21);
                Add(new[] { new Reservation { Quantity = 2 } }, new Reservation { Quantity = 1 }, 0);
            }
        }

        [Theory, ClassData(typeof(HappyPath))]
        public void CanAcceptReturnsReservationInHappyPathScenario(
            IEnumerable<Reservation> reservations,
            Reservation reservation,
            int capacitySurplus)
        {
            var reservedSeats = reservations.Sum(r => r.Quantity);
            var capacity =
                reservation.Quantity + reservedSeats + capacitySurplus;
            var sut = new MaîtreD(capacity);

            var actual = sut.CanAccept(reservations, reservation);

            Assert.True(actual);
        }

        [Property]
        public Property CanAcceptOnInsufficientCapacity()
        {
            return Prop.ForAll((
                from  r in GenerateReservation
                from eq in Arb.Default.PositiveInt().Generator
                from cs in Arb.Default.NonNegativeInt().Generator
                from rs in GenerateReservation.ListOf()
                select (r, eq.Item, cs.Item, rs)).ToArbitrary(),
                x =>
                {
                    var (reservation, excessQuantity, capacitySurplus, reservations) = x;
                    var reservedSeats = reservations.Sum(r => r.Quantity);
                    var quantity = capacitySurplus + excessQuantity;
                    var capacity = capacitySurplus + reservedSeats;
                    reservation.Quantity = quantity;
                    var sut = new MaîtreD(capacity);

                    var actual = sut.CanAccept(reservations, reservation);

                    Assert.False(actual);
                });
        }

        private static Gen<Reservation> GenerateReservation =>
            from d in Arb.Default.DateTime().Generator
            from e in Arb.Default.NonWhiteSpaceString().Generator
            from n in Arb.Default.NonWhiteSpaceString().Generator
            from q in Arb.Default.PositiveInt().Generator
            select new Reservation
            {
                Date = d,
                Email = e.Item,
                Name = n.Item,
                Quantity = q.Item
            };
    }
}
