using HotelBookings.Application.Extensions;
using HotelBookings.Common.DTOs;
using HotelBookings.Domain.Entities;
using HotelBookings.Domain.Interfaces;
using HotelBookings.Domain.Specifications;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace HotelBookings.Application.Services
{
    public class BookingService(IGenericRepository<Booking> bookingRepository,
                                IGenericRepository<Hotel> hotelRepository,
                                IGenericRepository<Room> roomRepository)
                 : IBookingService
    {
        public async Task<BookingDto?> GetBookingAsync (int id)
        {
            BaseSpecification<Booking> bookingSpec = new (b => b.Id.Equals(id));
            bookingSpec.AddInclude(b => b.Room);
            bookingSpec.AddInclude(b => b.Room.Hotel);
            var booking = await bookingRepository.GetWithSpecAsync(bookingSpec);
            return booking?.ToBookingDto();
        }

        public async Task<BookingResultDto> CreateBookingAsync(AvailabilityRequestDto bookingRequest)
        {
            var vaildatedBooking = await ValidateBooking(bookingRequest);

            if (!vaildatedBooking.Success)
            {
                return vaildatedBooking;
            }

            var hotel = await hotelRepository.GetByIdAsync(bookingRequest.HotelId.Value);

            if (hotel == null)
            {
                return CreateBookingResult(false, HttpStatusCode.UnprocessableEntity, $"No matching hotel for the given id of {bookingRequest.HotelId.Value}.");
			}

            bookingRequest.From = bookingRequest.From.AddHours(hotel.CheckInTime.Hour).AddMinutes(hotel.CheckInTime.Minute);
            bookingRequest.To = bookingRequest.To.AddHours(hotel.CheckOutTime.Hour).AddMinutes(hotel.CheckOutTime.Minute);

            BookedRoomsSpecification bookedRoomsSpec = new(bookingRequest);

            try
            {
                await bookingRepository.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);

                var bookedRoomIds = await bookingRepository.ExpressionToQuery(bookedRoomsSpec.Criteria).Select(b => b.RoomId).ToListAsync();

                if (bookedRoomIds.Contains(bookingRequest.RoomId.Value))
                {
                    return CreateBookingResult(false, HttpStatusCode.Conflict, "The requested room is not available for the given date range.");
                }

                // Create booking
                var booking = new Booking
                {
                    RoomId = bookingRequest.RoomId.Value,
                    HotelId = bookingRequest.HotelId.Value,
                    CheckIn = bookingRequest.From,
                    CheckOut = bookingRequest.To,
                    Guests = bookingRequest.Guests,
                    CreatedAt = DateTime.UtcNow
                };

                var rowAdded = await bookingRepository.AddAsync(booking);

                await bookingRepository.CommitTransactionAsync();

                return new BookingResultDto
                {
                    Success = true,
                    Booking = booking?.ToBookingDto()
                };
            }
            catch (Exception)
            {
                await bookingRepository.RollbackTransactionAsync();
                throw;
            }
        }

        private static BookingResultDto CreateBookingResult (bool success, HttpStatusCode? errorStatusCode = null, string? error = null) {
            return new BookingResultDto { Success = success, StatusCode = errorStatusCode, Message = error };
        }

        private async Task<BookingResultDto> ValidateBooking(AvailabilityRequestDto bookingRequest)
        {
            if (bookingRequest.Guests <= 0)
            {
                return CreateBookingResult(false, HttpStatusCode.UnprocessableEntity, "Guests must be greater than 0.");
            }

            if (bookingRequest.From >= bookingRequest.To)
            {
                return CreateBookingResult(false, HttpStatusCode.UnprocessableEntity, "Check-in must be before check-out.");
            }

            if (!bookingRequest.RoomId.HasValue)
            {
                return CreateBookingResult(false, HttpStatusCode.UnprocessableEntity, $"Required parameter '{nameof(bookingRequest.RoomId)}' has not been supplied.");
            }

            var requestedRoom = await roomRepository.GetByIdAsync(bookingRequest.RoomId.Value);

            if (requestedRoom == null)
            {
                return CreateBookingResult(false, HttpStatusCode.UnprocessableEntity, "The requested room does not exist.");
            }

            if (requestedRoom.Capacity < bookingRequest.Guests)
            {
                return CreateBookingResult(false,
                                           HttpStatusCode.UnprocessableEntity, 
                                           $"The requested room does not have sufficient capacity. There are {bookingRequest.Guests} guests " +
                                           $"and the room has a maximum capacity of {requestedRoom.Capacity}.");
            }

            return CreateBookingResult(true);        
        }
    }
}
