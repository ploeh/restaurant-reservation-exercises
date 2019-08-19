using System;

namespace Ploeh.Samples.BookingApi
{
    public class Reservation
    {
        public DateTime Date { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
    }
}