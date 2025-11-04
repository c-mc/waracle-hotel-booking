namespace HotelBookings.Common.DTOs
{
    public class BookingResultDto : BaseResultDto
    {
        public bool Success { get; set; }

        public BookingDto? Booking { get; set; }
    }
}
