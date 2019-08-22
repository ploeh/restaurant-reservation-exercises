using FsCheck;
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

        [Property]
        public Property InvalidDate(string date)
        {
            Action prop = () =>
            {
                var dto = new ReservationDto
                {
                    Date = date
                };

                var actual = Validator.Validate(dto);

                Assert.NotEmpty(actual);
            };
            return prop.When(!DateTime.TryParse(date, out _));
        }
    }
}
