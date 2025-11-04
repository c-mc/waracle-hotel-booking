using HotelBookings.Common.DTOs;
using HotelBookings.Domain.Entities;

namespace HotelBookings.Domain.Specifications
{
    public class BookedRoomsSpecification(AvailabilityRequestDto availabilityRequest)
        : BaseSpecification<Booking>(b => 
                    (
                        (availabilityRequest.To >= b.CheckIn && availabilityRequest.To <= b.CheckOut)
                        ||
                        (availabilityRequest.From >= b.CheckIn && availabilityRequest.From <= b.CheckOut)
                    )
            )
    {

    }
}
