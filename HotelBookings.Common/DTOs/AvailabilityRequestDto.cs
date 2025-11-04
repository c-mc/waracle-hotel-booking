namespace HotelBookings.Common.DTOs
{
    public class AvailabilityRequestDto
    {
        public Guid? HotelId { get; set; }

        public Guid? RoomId { get; set; }

        public int Guests { get; set; }
        
        public DateTime From { get; set; }
        
        public DateTime To { get; set; }
    }
}
