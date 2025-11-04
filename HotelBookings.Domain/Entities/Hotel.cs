namespace HotelBookings.Domain.Entities
{
    public class Hotel : BaseEntity
    {
        const Int16 MaxRooms = 6; // Limit rooms to 6, as per requirements
        private Int16 _roomsAdded = 0;
        private readonly List<Room> _rooms = [];

        public required string Name { get; set; }

        public string? Description { get; set; }
        
        public string? Address { get; set; }

        public ICollection<Room> Rooms { get { return _rooms; } }

        public TimeOnly CheckInTime { get; set; } = new TimeOnly(14, 00);

        public TimeOnly CheckOutTime { get; set; } = new TimeOnly(10, 00);

        public void AddRoom(Room room) {
            if (_roomsAdded < MaxRooms) {
                _rooms.Add(room);
                _roomsAdded++;
            }
        }
    }
}
