using HotelBookings.Domain.Enums;

namespace HotelBookings.Domain.Entities
{
    public class Room: BaseEntity
    {
        public required Guid HotelId { get; set; }

        public required RoomType Type { get; set; }
        
        public required int Capacity { get; set; }

        public required Hotel Hotel { get; set; }

        public ICollection<Booking> Bookings { get; set; } = [];
    }
}
