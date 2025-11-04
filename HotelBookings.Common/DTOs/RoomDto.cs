namespace HotelBookings.Common.DTOs
{
    public class RoomDto
    {
        public Guid Id { get; set; }

        public required string HotelName {  get; set; }

        public required string HotelAddress { get; set; }

        public required Guid HotelId { get; set; }

        public required string Type { get; set; }

        public required int Capacity { get; set; }
    }
}
