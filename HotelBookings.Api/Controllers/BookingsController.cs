using HotelBookings.Common.DTOs;
using HotelBookings.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookings.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController(IBookingService bookingService) : ControllerBase
    {
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [EndpointSummary("Get a booking by its ID")]
        public async Task<ActionResult<BookingDto>> GetBooking(int id)
        {
            var result = await bookingService.GetBookingAsync(id);

            return result != null ? Ok(result) : NotFound();
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(204)]
        [ProducesResponseType(409)]
        [ProducesResponseType(422)]
        [EndpointSummary("Create a new booking")]
        [EndpointDescription("Supply all of the parameters to attempt to create a booking. Dates only need to be in YYYY-MM-DD format.")]
        public async Task<ActionResult<BookingDto>> CreateBooking(AvailabilityRequestDto bookingRequest)
        {
            var bookingOutcome = await bookingService.CreateBookingAsync(bookingRequest);

            if (bookingOutcome == null) {
                return NoContent();
            }

            if (!bookingOutcome.Success) {
                return bookingOutcome.StatusCode switch
                {
                    System.Net.HttpStatusCode.Conflict => Conflict(bookingOutcome.Message),
                    System.Net.HttpStatusCode.UnprocessableEntity => UnprocessableEntity(bookingOutcome.Message),
                    _ => throw new ArgumentException($"Unexpected StatusCode of {bookingOutcome.StatusCode} returned in CreateBooking."),
                };
            }

            return CreatedAtAction(nameof(GetBooking), new { id = bookingOutcome?.Booking?.Id }, bookingOutcome?.Booking);
        }
    }
}
