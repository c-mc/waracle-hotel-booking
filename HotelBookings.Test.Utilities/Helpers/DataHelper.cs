using HotelBookings.Common.DTOs;
using HotelBookings.Domain.Entities;
using HotelBookings.Domain.Enums;

namespace HotelBookings.Test.Utilities.Helpers
{
    public static class DataHelper
    {
        public static Hotel CreateHotelWithRoom(RoomType roomType = RoomType.Double, int capacity = 2)
        {
            var hotel = new Hotel { Id = Guid.NewGuid(), Name = "Test Hotel", Description = "A hotel created for testing", Address = "1 Test Hotel Lane" };

            hotel.AddRoom(new Room { Id = Guid.NewGuid(), HotelId = hotel.Id, Capacity = capacity, Type = roomType, Hotel = hotel });

            return hotel;
        }

        public static Booking CreateBooking(Hotel hotel, DateTime dateFrom, DateTime dateTo, int numberOfGuests)
        {
            return new Booking
            {
                Id = 1,
                HotelId = hotel.Id,
                RoomId = hotel.Rooms.First().Id,
                CheckIn = dateFrom,
                CheckOut = dateTo,
                Guests = numberOfGuests,
                CreatedAt = DateTime.UtcNow
            };
        }

        public static AvailabilityRequestDto CreateBookingRequest(Hotel hotel, DateTime dateFrom, DateTime dateTo, int numberOfGuests)
        {
            return new AvailabilityRequestDto
            {
                HotelId = hotel.Id,
                RoomId = hotel.Rooms.First().Id,
                From = dateFrom,
                To = dateTo,
                Guests = numberOfGuests
            };
        }
    }
}
