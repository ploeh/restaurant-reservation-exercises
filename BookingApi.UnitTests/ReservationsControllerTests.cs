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

        [Property]
        public void PostValidDtoWhenNoPriorReservationsExist(
            NonNegativeInt capacitySurplus,
            PositiveInt quantity)
        {
            var repository = new FakeReservationsRepository();
            var capacity = capacitySurplus.Item + quantity.Item;
            var sut = new ReservationsController(repository, capacity);

            var dto = new ReservationDto
            {
                Date = "2019-08-20",
                Quantity = quantity.Item
            };
            var actual = sut.Post(dto);

            Assert.IsAssignableFrom<OkObjectResult>(actual);
            Assert.NotEmpty(repository);
        }

        [Property]
        public void PostValidDtoWhenSoldOut(
            PositiveInt capacity,
            PositiveInt excessQuantity)
        {
            var repository = new FakeReservationsRepository();
            var quantity = capacity.Item + excessQuantity.Item;
            var sut = new ReservationsController(repository, capacity.Item);

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
