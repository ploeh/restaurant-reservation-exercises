using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ploeh.Samples.BookingApi
{
    public class MaîtreD : IMaîtreD
    {
        public MaîtreD(int capacity, IReservationsRepository repository)
        {
            Capacity = capacity;
            Repository = repository;
        }

        public int Capacity { get; }
        public IReservationsRepository Repository { get; }

        public void TryAccept(Reservation reservation)
        {
            var reservations = Repository.ReadReservations(reservation.Date);
            var reservedSeats = reservations.Sum(r => r.Quantity);

            if (Capacity < reservedSeats + reservation.Quantity)
                throw new InvalidOperationException("Insufficient capacity.");

            Repository.Create(reservation);
        }
    }
}
