namespace Ploeh.Samples.BookingApi
{
    public interface IValidator
    {
        string Validate(ReservationDto dto);
    }
}