using FsCheck;
using FsCheck.Xunit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Ploeh.Samples.BookingApi.UnitTests
{
    public class ReservationsControllerTests
    {
        [Property]
        public void PostInvalidDto(PositiveInt capacity)
        {
            var sut = new ReservationsController(
                new FakeReservationsRepository(),
                capacity.Item);

            var dto = new ReservationDto { };
            var actual = sut.Post(dto);

            var br = Assert.IsAssignableFrom<BadRequestObjectResult>(actual);
            var msg = Assert.IsAssignableFrom<string>(br.Value);
            Assert.NotEmpty(msg);
        }

        [Theory]
        [InlineData(10, 1)]
        [InlineData( 9, 9)]
        [InlineData(30, 4)]
        public void PostValidDtoWhenNoPriorReservationsExist(
            int capacity,
            int quantity)
        {
            var repository = new FakeReservationsRepository();
            var sut = new ReservationsController(repository, capacity);

            var dto = new ReservationDto
            {
                Date = "2019-08-20",
                Quantity = quantity
            };
            var actual = sut.Post(dto);

            Assert.IsAssignableFrom<OkObjectResult>(actual);
            Assert.NotEmpty(repository);
        }

        [Theory]
        [InlineData( 1,  2)]
        [InlineData( 1,  3)]
        [InlineData(11, 15)]
        public void PostValidDtoWhenSoldOut(int capacity, int quantity)
        {
            var repository = new FakeReservationsRepository();
            var sut = new ReservationsController(repository, capacity);

            var dto = new ReservationDto
            {
                Date = "2019-08-20",
                Quantity = quantity
            };
            var actual = sut.Post(dto);

            var c = Assert.IsAssignableFrom<ObjectResult>(actual);
            Assert.Equal(StatusCodes.Status500InternalServerError, c.StatusCode);
        }
    }
}
