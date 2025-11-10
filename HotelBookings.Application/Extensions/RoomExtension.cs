using HotelBookings.Common.DTOs;
using HotelBookings.Domain.Entities;

namespace HotelBookings.Application.Extensions
{
    internal static class RoomExtension
    {
        /// <summary>
        /// Simple extension to map Room entity to RoomDto
        /// </summary>
        /// <param name="room"></param>
        internal static RoomDto ToRoomDto(this Room room)
        {
            return new RoomDto
            {
                Id = room.Id,
                HotelName = room.Hotel.Name,
                HotelAddress = room.Hotel.Address ?? string.Empty,
                Type = room?.Type.ToString() ?? string.Empty,
                Capacity = room.Capacity,
                HotelId = room.Hotel.Id
            };
        }

        /// <summary>
        /// Simple extension to map IReadOnlyList<Room> entities to IReadOnlyList<RoomDto>
        /// </summary>
        /// <param name="rooms"></param>
        internal static IReadOnlyList<RoomDto> ToRoomDtoList(this IReadOnlyList<Room> rooms)
        {
            return [.. rooms.Select(r => r.ToRoomDto())];
        }
    }
}