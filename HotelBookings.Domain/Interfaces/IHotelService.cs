using HotelBookings.Common.DTOs;

namespace HotelBookings.Domain.Interfaces
{
    public interface IHotelService
    {
        Task<IReadOnlyList<HotelBaseDto>> SearchHotelsAsync(SearchRequestDto searchRequest);

        Task<IReadOnlyList<RoomDto>> GetAvailableRoomsAsync(AvailabilityRequestDto availabilityRequest);

        Task<HotelDto?> GetHotelAsync(Guid id);

        Task<IReadOnlyList<RoomDto>?> GetHotelRoomsAsync(Guid hotelId);
    }
}
