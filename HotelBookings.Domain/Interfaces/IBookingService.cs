using HotelBookings.Common.DTOs;

namespace HotelBookings.Domain.Interfaces
{
    public interface IBookingService
    {
        Task<BookingDto?> GetBookingAsync(int id);

        Task<BookingResultDto> CreateBookingAsync(AvailabilityRequestDto bookingRequest);
    }
}
