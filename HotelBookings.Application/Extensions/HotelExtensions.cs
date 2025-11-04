using HotelBookings.Common.DTOs;
using HotelBookings.Domain.Entities;
using System.Net;
using System.Xml.Linq;

namespace HotelBookings.Application.Extensions
{
    internal static class HotelExtensions
    {
        /// <summary>
        /// Simple extension to map Hotel entity to HotelBaseDto
        /// </summary>
        /// <param name="hotel"></param>
        internal static HotelBaseDto ToHotelBaseDto(this Hotel hotel)
        {
            return new HotelBaseDto
            {
                Id = hotel.Id,
                Name = hotel.Name,
                Description = hotel.Description,
                Address = hotel.Address
            };
        }

        /// <summary>
        /// Simple extension to map IReadOnlyList<Hotel> entities to IReadOnlyList<HotelSearchResultDto>
        /// </summary>
        /// <param name="hotels"></param>
        internal static IReadOnlyList<HotelBaseDto> ToHotelBaseDtoList(this IReadOnlyList<Hotel> hotels)
        {
            return [.. hotels.Select(h => h.ToHotelBaseDto())];
        }

        /// <summary>
        /// Simple extension to map Hotel entity to HotelDto
        /// </summary>
        /// <param name="hotel"></param>
        internal static HotelDto ToHotelDto(this Hotel hotel)
        {
            return new HotelDto
            {
                Id = hotel.Id,
                Name = hotel.Name,
                Description = hotel.Description,
                Address = hotel.Address,
                CheckInTime = hotel.CheckInTime,
                CheckOutTime = hotel.CheckOutTime
            };
        }

        /// <summary>
        /// Simple extension to map IReadOnlyList<Hotel> entities to IReadOnlyList<HotelDto>
        /// </summary>
        /// <param name="hotels"></param>
        internal static IReadOnlyList<HotelDto> ToHotelDtoList(this IReadOnlyList<Hotel> hotels)
        {
            return [.. hotels.Select(h => h.ToHotelDto())];
        }
    }
}