﻿using Microsoft.AspNetCore.Http;
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
            var dto = new ReservationDto { };
            var validatorTD = new Mock<IValidator>();
            validatorTD.Setup(v => v.Validate(dto)).Returns("Boo!");
            var sut = new ReservationsController(
                validatorTD.Object,
                new Mock<IMapper>().Object,
                new Mock<IMaîtreD>().Object,
                new Mock<IReservationsRepository>().Object);

            var actual = sut.Post(dto);

            var br = Assert.IsAssignableFrom<BadRequestObjectResult>(actual);
            Assert.Equal("Boo!", br.Value);
        }

        [Fact]
        public void PostValidDtoWhenEnoughCapacityExists()
        {
            var dto = new ReservationDto { };
            var r = new Reservation { };
            var validatorTD = new Mock<IValidator>();
            validatorTD.Setup(v => v.Validate(dto)).Returns("");
            var mapperTD = new Mock<IMapper>();
            mapperTD.Setup(m => m.Map(dto)).Returns(r);
            var maîtreDTD = new Mock<IMaîtreD>();
            maîtreDTD
                .Setup(m => m
                    .CanAccept(It.IsAny<IEnumerable<Reservation>>(), r))
                .Returns(true);
            var repositoryTD = new Mock<IReservationsRepository>();
            repositoryTD.Setup(repo => repo.Create(r)).Returns(1337);
            var sut = new ReservationsController(
                validatorTD.Object,
                mapperTD.Object,
                maîtreDTD.Object,
                repositoryTD.Object);

            var actual = sut.Post(dto);

            var ok = Assert.IsAssignableFrom<OkObjectResult>(actual);
            Assert.Equal(1337, ok.Value);
        }

        [Fact]
        public void PostValidDtoWhenSoldOut()
        {
            var dto = new ReservationDto { };
            var r = new Reservation { };
            var validatorTD = new Mock<IValidator>();
            validatorTD.Setup(v => v.Validate(dto)).Returns("");
            var mapperTD = new Mock<IMapper>();
            mapperTD.Setup(m => m.Map(dto)).Returns(r);
            var maîtreDTD = new Mock<IMaîtreD>();
            maîtreDTD
                .Setup(m => m
                    .CanAccept(It.IsAny<IEnumerable<Reservation>>(), r))
                .Returns(false);
            var sut = new ReservationsController(
                validatorTD.Object,
                mapperTD.Object,
                maîtreDTD.Object,
                new Mock<IReservationsRepository>().Object);

            var actual = sut.Post(dto);

            var c = Assert.IsAssignableFrom<ObjectResult>(actual);
            Assert.Equal(StatusCodes.Status500InternalServerError, c.StatusCode);
        }
    }
}
