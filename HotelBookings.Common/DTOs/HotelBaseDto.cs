namespace HotelBookings.Common.DTOs
{
    public class HotelBaseDto
    {
        public required Guid Id { get; set; }
        
        public required string Name { get; set; }

        public string? Description { get; set; }

        public string? Address { get; set; }

    }
}
