using HotelBookings.Application.Services;
using HotelBookings.Common.DTOs;
using HotelBookings.Domain.Entities;
using HotelBookings.Domain.Enums;
using HotelBookings.Domain.Interfaces;
using HotelBookings.Domain.Specifications;
using HotelBookings.Test.Utilities.Helpers;
using Moq;
using System.Net;

namespace HotelBookings.Application.Tests.Services;

[TestClass]
public class BookingServiceTests
{
    [TestMethod]
    public async Task Check_Cant_Book_A_Room_When_Its_Already_Booked()
    {
        // Arrange
        var numberOfGuests = 1;
        var hotel = DataHelper.CreateHotelWithRoom(RoomType.Single, numberOfGuests);
        var dateFrom = new DateTime(2026, 01, 01);
        var dateTo = new DateTime(2026, 01, 02);
        var booking = DataHelper.CreateBooking(hotel, dateFrom, dateTo, numberOfGuests);

        var hotelRepositoryMock = new Mock<IGenericRepository<Hotel>>();
        var roomRepositoryMock = new Mock<IGenericRepository<Room>>();
        var bookingRepositoryMock = new Mock<IBookingRepository>();        

        hotelRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(hotel);

        roomRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(hotel.Rooms.First());

        bookingRepositoryMock.Setup(x => x.IsRoomAlreadyBooked(It.IsAny<AvailabilityRequestDto>()))
            .ReturnsAsync(true);

        var bookingService = new BookingService(bookingRepositoryMock.Object, hotelRepositoryMock.Object, roomRepositoryMock.Object);
        
        var bookingRequest = DataHelper.CreateBookingRequest(hotel, dateFrom, dateTo, numberOfGuests);

        // Action
        var bookingResponse = await bookingService.CreateBookingAsync(bookingRequest);

        // Assert
        Assert.IsFalse(bookingResponse.Success);
        Assert.AreEqual(HttpStatusCode.Conflict, bookingResponse.StatusCode);
        Assert.AreEqual("The requested room is not available for the given date range.", bookingResponse.Message);
    }

    [TestMethod]
    public async Task Check_Cant_Book_A_Room_With_Insufficient_Capacity()
    {
        // Arrange
        var numberOfGuests = 2;
        var roomCapacity = 1;

        // Action
        var bookingResponse = await CreateBooking(numberOfGuests, roomCapacity);

        // Assert
        Assert.IsFalse(bookingResponse.Success);
        Assert.AreEqual(HttpStatusCode.UnprocessableEntity, bookingResponse.StatusCode);
        Assert.AreEqual($"The requested room does not have sufficient capacity. There are {numberOfGuests} guests " +
                        $"and the room has a maximum capacity of {roomCapacity}.", bookingResponse.Message);
    }

    [TestMethod]
    public async Task Check_Can_Book_A_Room()
    {
        // Arrange
        var numberOfGuests = 1;
        var roomCapacity = 1;

        // Action
        var bookingResponse = await CreateBooking(numberOfGuests, roomCapacity);

        // Assert
        Assert.IsTrue(bookingResponse.Success);
        Assert.IsNotNull(bookingResponse.Booking);
    }

    [TestMethod]
    public async Task Check_Can_Book_A_Room_With_Greater_Capacity_Than_Guests()
    {
        // Arrange
        var numberOfGuests = 1;
        var roomCapacity = 2;

        // Action
        var bookingResponse = await CreateBooking(numberOfGuests, roomCapacity);

        // Assert
        Assert.IsTrue(bookingResponse.Success);
        Assert.IsNotNull(bookingResponse.Booking);
    }

    [TestMethod]
    public async Task Check_Can_Get_Existing_Booking_By_Id()
    {
        // Arrange
        var numberOfGuests = 1;
        var hotel = DataHelper.CreateHotelWithRoom(RoomType.Single, numberOfGuests);
        var dateFrom = new DateTime(2026, 01, 01);
        var dateTo = new DateTime(2026, 01, 02);
        var booking = DataHelper.CreateBooking(hotel, dateFrom, dateTo, numberOfGuests);

        var hotelRepositoryMock = new Mock<IGenericRepository<Hotel>>();
        var roomRepositoryMock = new Mock<IGenericRepository<Room>>();
        var bookingRepositoryMock = new Mock<IBookingRepository>();

        bookingRepositoryMock.Setup(x => x.GetWithSpecAsync(It.IsAny<BaseSpecification<Booking>>())).ReturnsAsync(booking);

        var bookingService = new BookingService(bookingRepositoryMock.Object, hotelRepositoryMock.Object, roomRepositoryMock.Object);

        // Action
        var bookingDto = await bookingService.GetBookingAsync(booking.Id);

        // Assert
        Assert.IsNotNull(bookingDto);
        Assert.AreEqual(booking.Id, bookingDto.Id);
    }

    private static async Task<BookingResultDto> CreateBooking(int numberOfGuests, int roomCapacity)
    {
        // Arrange
        var hotel = DataHelper.CreateHotelWithRoom(RoomType.Single, roomCapacity);
        var dateFrom = new DateTime(2026, 01, 01);
        var dateTo = new DateTime(2026, 01, 02);
        var booking = DataHelper.CreateBooking(hotel, dateFrom, dateTo, numberOfGuests);

        var hotelRepositoryMock = new Mock<IGenericRepository<Hotel>>();
        var roomRepositoryMock = new Mock<IGenericRepository<Room>>();
        var bookingRepositoryMock = new Mock<IBookingRepository>();

        hotelRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(hotel);

        roomRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(hotel.Rooms.First());

        bookingRepositoryMock.Setup(x => x.IsRoomAlreadyBooked(It.IsAny<AvailabilityRequestDto>()))
            .ReturnsAsync(false);

        var bookingService = new BookingService(bookingRepositoryMock.Object, hotelRepositoryMock.Object, roomRepositoryMock.Object);

        var bookingRequest = DataHelper.CreateBookingRequest(hotel, dateTo.AddDays(1), dateTo.AddDays(2), numberOfGuests);

        // Action
        return await bookingService.CreateBookingAsync(bookingRequest);
    }
}
