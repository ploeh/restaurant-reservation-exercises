using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ploeh.Samples.BookingApi
{
    public interface IMaîtreD
    {
        bool CanAccept(
            IEnumerable<Reservation> reservations,
            Reservation reservation);
    }
}
