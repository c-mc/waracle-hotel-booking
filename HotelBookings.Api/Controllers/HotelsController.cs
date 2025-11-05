using HotelBookings.Common.DTOs;
using HotelBookings.Domain.Entities;
using HotelBookings.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookings.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController(IHotelService hotelService) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [EndpointSummary("Search for hotels by name")]
        [EndpointDescription(
            "Call this endpoint to search hotels by name or leave the Name blank to return all hotels. " +
            "If a Name is supplied, the search is case insensitive and will return hotels where any part of the hotel's name matches. " +
            "There is also the option to paginate the results returned by using PageIndex (0 offset) and PageSize parameters")]
        public async Task<ActionResult<HotelBaseDto>> SearchHotels([FromQuery] SearchRequestDto searchRequest)
        {
            var matchingHotels = await hotelService.SearchHotelsAsync(searchRequest);

            return matchingHotels.Any() ? Ok(matchingHotels) : NoContent();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [EndpointSummary("Get a hotel by its ID")]
        public async Task<ActionResult<HotelDto>> GetHotel(Guid id)
        {
            var hotel = await hotelService.GetHotelAsync(id);

            return hotel != null ? Ok(hotel) : NotFound();
        }

        [HttpGet("{id}/rooms")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [EndpointSummary("Get all rooms for a hotel by its ID")]
        public async Task<ActionResult<IReadOnlyList<RoomDto>>> GetHotelRooms(Guid id)
        {
            var hotelRooms = await hotelService.GetHotelRoomsAsync(id);

            return hotelRooms != null ? Ok(hotelRooms) : NotFound();
        }

        [HttpGet("{id}/availability")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [EndpointSummary("Get available rooms at a given hotel")]
        [EndpointDescription("Call this endpoint to find available rooms, in a given hotel, that meet the criteria of the supplied parameters." +
                             "Any dates supplied can be in YYYY-MM-DD format.")]
        public async Task<ActionResult<RoomDto>> GetAvailability(Guid id, Guid? roomId, int guests, DateOnly from, DateOnly to)
        {
            var availabilityRequest = new AvailabilityRequestDto { 
                HotelId = id, 
                RoomId = roomId, 
                Guests = guests, 
                From = from.ToDateTime(TimeOnly.Parse("00:00")), 
                To = to.ToDateTime(TimeOnly.Parse("00:00")) 
            };

            var results = await hotelService.GetAvailableRoomsAsync(availabilityRequest);

            return results.Any() ? Ok(results) : NoContent();
        }

        [HttpGet("availability")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [EndpointSummary("Get available rooms all hotels")]
        [EndpointDescription("Call this endpoint to find all available rooms that meet the criteria of the supplied parameters. Dates supplied can be in YYYY-MM-DD format.")]
        public async Task<ActionResult<RoomDto>> GetAvailabilityForAllHotels(Guid? roomId, int guests, DateOnly from, DateOnly to)
        {
            var availabilityRequest = new AvailabilityRequestDto
            {
                RoomId = roomId,
                Guests = guests,
                From = from.ToDateTime(TimeOnly.Parse("00:00")),
                To = to.ToDateTime(TimeOnly.Parse("00:00"))
            };

            var results = await hotelService.GetAvailableRoomsAsync(availabilityRequest);

            return results.Any() ? Ok(results) : NoContent();
        }
    }
}
