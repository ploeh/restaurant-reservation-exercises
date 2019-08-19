using System;
using System.Collections.Generic;

namespace Ploeh.Samples.BookingApi
{
    public interface IReservationsRepository
    {
        IEnumerable<Reservation> ReadReservations(DateTime date);
        int Create(Reservation reservation);
    }
}