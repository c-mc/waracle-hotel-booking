namespace HotelBookings.Common.DTOs
{
    public class HotelDto : HotelBaseDto
    {
        public TimeOnly CheckInTime { get; set; }

        public TimeOnly CheckOutTime { get; set; }
    }
}
