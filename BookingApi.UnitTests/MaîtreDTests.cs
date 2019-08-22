using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Ploeh.Samples.BookingApi.UnitTests
{
    public class MaîtreDTests
    {
        private class HappyPath : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { new DateTime(2018,  8, 30),  4, 10 };
                yield return new object[] { new DateTime(2019,  9, 29), 10, 10 };
                yield return new object[] { new DateTime(2020, 10, 28), 20, 20 };
                yield return new object[] { new DateTime(2021, 11, 27),  1, 22 };
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        [Theory, ClassData(typeof(HappyPath))]
        public void CanAcceptReturnsReservationInHappyPathScenario(
            DateTime date,
            int quantity,
            int capacity)
        {
            var reservation = new Reservation
            {
                Date = date,
                Quantity = quantity
            };
            var sut = new MaîtreD(capacity);

            var actual = sut.CanAccept(new Reservation[0], reservation);

            Assert.True(actual);
        }

        [Theory]
        [InlineData( 4, 10)]
        [InlineData( 3,  3)]
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
