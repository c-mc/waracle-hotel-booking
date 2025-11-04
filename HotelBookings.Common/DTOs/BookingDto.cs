namespace HotelBookings.Common.DTOs
{
    public class BookingDto
    {
        public int Id { get; set; }

        public required string HotelName {  get; set; }

        public required string HotelAddress { get; set; }

        public required string RoomType { get; set; }

        public DateTime CheckIn { get; set; }

        public DateTime CheckOut { get; set; }

        public int Guests { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
