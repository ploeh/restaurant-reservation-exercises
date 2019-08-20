using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ploeh.Samples.BookingApi
{
    public static class Validator
    {
        public static string Validate(ReservationDto dto)
        {
            if (!DateTime.TryParse(dto.Date, out var _))
                return $"Invalid date: {dto.Date}.";
            return "";
        }
    }
}
