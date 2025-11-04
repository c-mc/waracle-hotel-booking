namespace HotelBookings.Domain.Entities
{
    public abstract class BaseEntity
    {
        public required Guid Id { get; set; }
    }
}
