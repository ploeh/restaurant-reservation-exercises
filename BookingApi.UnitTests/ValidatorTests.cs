using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Ploeh.Samples.BookingApi.UnitTests
{
    public class ValidatorTests
    {
        [Fact]
        public void ValidDate()
        {
            var dto = new ReservationDto
            {
                Date = "2018-08-30T19:47:00"
            };

            var actual = Validator.Validate(dto);

            Assert.Empty(actual);
        }

        [Fact]
        public void InvalidDate()
        {
            var dto = new ReservationDto
            {
                Date = "Invalid date"
            };

            var actual = Validator.Validate(dto);

            Assert.NotEmpty(actual);
        }
    }
}
