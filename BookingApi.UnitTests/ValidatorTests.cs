using FsCheck.Xunit;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Ploeh.Samples.BookingApi.UnitTests
{
    public class ValidatorTests
    {
        [Property]
        public void ValidDate(DateTime date)
        {
            var dto = new ReservationDto
            {
                Date = date.ToString()
            };

            var actual = Validator.Validate(dto);

            Assert.Empty(actual);
        }

        [Theory]
        [InlineData("Invalid date")]
        [InlineData("foo")]
        [InlineData("  ")]
        public void InvalidDate(string date)
        {
            var dto = new ReservationDto
            {
                Date = date
            };

            var actual = Validator.Validate(dto);

            Assert.NotEmpty(actual);
        }
    }
}
