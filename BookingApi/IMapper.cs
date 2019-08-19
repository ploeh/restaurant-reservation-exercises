namespace Ploeh.Samples.BookingApi
{
    public interface IMapper
    {
        Reservation Map(ReservationDto dto);
    }
}