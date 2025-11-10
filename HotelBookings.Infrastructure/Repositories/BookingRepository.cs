using HotelBookings.Common.DTOs;
using HotelBookings.Domain.Entities;
using HotelBookings.Domain.Interfaces;
using HotelBookings.Domain.Specifications;
using HotelBookings.Infrastructure.Data;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace HotelBookings.Infrastructure.Repositories
{
    public class BookingRepository(HotelBookingContext dbContext) : GenericRepository<Booking>(dbContext), IBookingRepository
    {
        public async Task<bool> IsRoomAlreadyBooked(AvailabilityRequestDto bookingRequest)
        {
            BookedRoomsSpecification bookedRoomsSpec = new(bookingRequest);
            bookedRoomsSpec.Criteria.And(r => r.RoomId == bookingRequest.RoomId);
            return await ExpressionToQuery(bookedRoomsSpec.Criteria).Take(1).AnyAsync();
        }   
    }
}
