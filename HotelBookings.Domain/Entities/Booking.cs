namespace HotelBookings.Domain.Entities
{
    public class Booking
    {        
        public int Id { get; set; }

        public Guid HotelId { get; set; }
        
        public Guid RoomId { get; set; }
        
        public Room Room { get; set; } = null!;
        
        public DateTime CheckIn { get; set; }
        
        public DateTime CheckOut { get; set; }
        
        public int Guests { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
