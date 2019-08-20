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
        public void TryAcceptCreatesReservationIdInHappyPathScenario()
        {
            var reservation = new Reservation
            {
                Date = new DateTime(2018, 8, 30),
                Quantity = 4
            };
            var td = new Mock<IReservationsRepository>();
            td
                .Setup(r => r.ReadReservations(reservation.Date))
                .Returns(new Reservation[0]);
            var sut = new MaîtreD(capacity: 10, td.Object);

            sut.TryAccept(reservation);

            td.Verify(r => r.Create(reservation));
        }

        [Fact]
        public void TryAcceptOnInsufficientCapacity()
        {
            var reservation = new Reservation
            {
                Date = new DateTime(2018, 8, 30),
                Quantity = 4
            };
            var td = new Mock<IReservationsRepository>();
            td
                .Setup(r => r.ReadReservations(reservation.Date))
                .Returns(new[] { new Reservation { Quantity = 7 } });
            var sut = new MaîtreD(capacity: 10, td.Object);

            Assert.Throws<InvalidOperationException>(() =>
                sut.TryAccept(reservation));

            td.Verify(r => r.Create(It.IsAny<Reservation>()), Times.Never);
        }
    }
}
