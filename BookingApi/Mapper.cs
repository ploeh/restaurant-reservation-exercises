using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ploeh.Samples.BookingApi
{
    public static class Mapper
    {
        public static Reservation Map(ReservationDto dto)
        {
            return new Reservation
            {
                Date = DateTime.Parse(dto.Date),
                Email = dto.Email,
                Name = dto.Name,
                Quantity = dto.Quantity
            };
        }
    }
}
