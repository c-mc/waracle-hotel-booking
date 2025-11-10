using HotelBookings.Common.DTOs;
using HotelBookings.Domain.Entities;

namespace HotelBookings.Domain.Interfaces
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        Task<bool> IsRoomAlreadyBooked(AvailabilityRequestDto bookingRequest);
    }
}
