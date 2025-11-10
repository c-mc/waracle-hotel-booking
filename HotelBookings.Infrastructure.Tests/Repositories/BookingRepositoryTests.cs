using HotelBookings.Common.DTOs;
using HotelBookings.Domain.Entities;
using HotelBookings.Domain.Enums;
using HotelBookings.Infrastructure.Data;
using HotelBookings.Infrastructure.Repositories;
using HotelBookings.Test.Utilities.Helpers;
using Microsoft.EntityFrameworkCore;

namespace HotelBookings.Infrastructure.Tests.Repositories;

[TestClass]
public class BookingRepositoryTests
{
    [TestMethod]
    public async Task Check_IsRoomAlreadyBooked_Response_When_Room_Is_Already_Booked()
    {
        // Arrange
        int numberOfGuests = 1;
        var hotel = DataHelper.CreateHotelWithRoom(RoomType.Single, numberOfGuests);
        var dateFrom = new DateTime(2026, 01, 10);
        var dateTo = dateFrom.AddDays(7d);
        var booking = DataHelper.CreateBooking(hotel, dateFrom, dateTo, numberOfGuests);

        var bookingRequestSameDates = DataHelper.CreateBookingRequest(hotel, dateFrom, dateTo, numberOfGuests);
        var bookingRequestDateToClash = DataHelper.CreateBookingRequest(hotel, dateFrom.AddDays(-2d), dateTo.AddDays(-2d), numberOfGuests);
        var bookingRequestDateFromClash = DataHelper.CreateBookingRequest(hotel, dateFrom.AddDays(2d), dateTo.AddDays(2d), numberOfGuests);
        var bookingRequestDatesBetweenClash = DataHelper.CreateBookingRequest(hotel, dateFrom.AddDays(2d), dateTo.AddDays(-2d), numberOfGuests);
        var bookingRequestDatesEitherSideClash = DataHelper.CreateBookingRequest(hotel, dateFrom.AddDays(-2d), dateTo.AddDays(2d), numberOfGuests);

        // Action
        var isRoomBookedSameDates = await GetIsRoomAlreadyBookedResponse(hotel, booking, bookingRequestSameDates);
        var isRoomBookedDateToClash = await GetIsRoomAlreadyBookedResponse(hotel, booking, bookingRequestDateToClash);
        var isRoomBookedDateFromClash = await GetIsRoomAlreadyBookedResponse(hotel, booking, bookingRequestDateFromClash);
        var isRoomBookedDatesBetweenClash = await GetIsRoomAlreadyBookedResponse(hotel, booking, bookingRequestDatesBetweenClash);
        var isRoomBookedDatesEitherSideClash = await GetIsRoomAlreadyBookedResponse(hotel, booking, bookingRequestDatesEitherSideClash);

        // Assert
        Assert.IsTrue(isRoomBookedSameDates);
        Assert.IsTrue(isRoomBookedDateToClash);
        Assert.IsTrue(isRoomBookedDateFromClash);
        Assert.IsTrue(isRoomBookedDatesBetweenClash);
        Assert.IsTrue(isRoomBookedDatesEitherSideClash);
    }

    [TestMethod]
    public async Task Check_IsRoomAlreadyBooked_Response_When_Room_Is_Not_Already_Booked()
    {
        // Arrange
        int numberOfGuests = 1;
        var hotel = DataHelper.CreateHotelWithRoom(RoomType.Single, numberOfGuests);
        var dateFrom = new DateTime(2026, 01, 01);
        var dateTo = new DateTime(2026, 01, 02);
        var bookingRequest = DataHelper.CreateBookingRequest(hotel, dateFrom, dateTo, numberOfGuests);

        // Action
        var isRoomBooked = await GetIsRoomAlreadyBookedResponse(hotel, null, bookingRequest);

        // Assert
        Assert.IsFalse(isRoomBooked);
    }

    private static async Task<bool> GetIsRoomAlreadyBookedResponse(Hotel hotel, Booking? booking, AvailabilityRequestDto bookingRequest)
    {
        using var context = new HotelBookingContext(DatabaseHelper.ContextOptions());

        context.Database.EnsureCreated();

        try
        {
            await context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
            context.Hotels.Add(hotel);
            if (booking != null)
            {
                context.Bookings.Add(booking);
            }
            context.SaveChanges();

            var bookingRepository = new BookingRepository(context);

            return await bookingRepository.IsRoomAlreadyBooked(bookingRequest);
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            // Reset Database
            await context.Database.RollbackTransactionAsync();
        }
    }
}
