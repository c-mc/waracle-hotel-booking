using HotelBookings.Common.DTOs;
using HotelBookings.Domain.Entities;

namespace HotelBookings.Application.Extensions
{
    internal static class BookingExtensions
    {
        /// <summary>
        /// Simple extension to map Booking entity to BookingDto
        /// </summary>
        /// <param name="booking"></param>
        internal static BookingDto ToBookingDto(this Booking booking)
        {
            return new BookingDto
            {
                Id = booking.Id,
                HotelName = booking.Room?.Hotel?.Name,
                HotelAddress = booking.Room?.Hotel?.Address,
                RoomType = booking.Room?.Type.ToString(),
                CheckIn = booking.CheckIn,
                CheckOut = booking.CheckOut,
                Guests = booking.Guests,
                CreatedAt = booking.CreatedAt
            };
        }
    }
}