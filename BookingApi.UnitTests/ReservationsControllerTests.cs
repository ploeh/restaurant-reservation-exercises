using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Ploeh.Samples.BookingApi.UnitTests
{
    public class ReservationsControllerTests
    {
        [Fact]
        public void PostInvalidDto()
        {
            // properly initialize `dto` here:
            ReservationDto dto = null;
            // properly initialise `sut` here:
            ReservationsController sut = null;

            var actual = sut.Post(dto);

            var br = Assert.IsAssignableFrom<BadRequestObjectResult>(actual);
            // Uncomment and assign a proper value to `expected`:
            //Assert.Equal(expected, br.Value);
        }

        [Fact]
        public void PostValidDtoWhenEnoughCapacityExists()
        {
            // properly initialize `dto` here:
            ReservationDto dto = null;
            // properly initialise `sut` here:
            ReservationsController sut = null;

            var actual = sut.Post(dto);

            var ok = Assert.IsAssignableFrom<OkObjectResult>(actual);
            // Uncomment and assign a proper value to `expected`:
            //Assert.Equal(expected, ok.Value);
        }

        [Fact]
        public void PostValidDtoWhenSoldOut()
        {
            // properly initialize `dto` here:
            ReservationDto dto = null;
            // properly initialise `sut` here:
            ReservationsController sut = null;

            var actual = sut.Post(dto);

            var c = Assert.IsAssignableFrom<ObjectResult>(actual);
            // Uncomment and assign a proper value to `expected`:
            //Assert.Equal(expected, c.StatusCode);
        }
    }
}
