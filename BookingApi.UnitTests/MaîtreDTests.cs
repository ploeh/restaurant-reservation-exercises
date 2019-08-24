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
        [Property]
        public Property CanAcceptReturnsReservationInHappyPathScenario()
        {
            return Prop.ForAll((
                from rs in GenerateReservation.ListOf()
                from r in GenerateReservation
                from cs in Arb.Default.NonNegativeInt().Generator
                select (rs, r, cs.Item)).ToArbitrary(),
                x =>
                {
                    var (reservations, reservation, capacitySurplus) = x;
                    var reservedSeats = reservations.Sum(r => r.Quantity);
                    var capacity =
                        reservation.Quantity + reservedSeats + capacitySurplus;
                    var sut = new MaîtreD(capacity);

                    var actual = sut.CanAccept(reservations, reservation);

                    Assert.True(actual);
                });
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
