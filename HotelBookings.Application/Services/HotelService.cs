using HotelBookings.Application.Extensions;
using HotelBookings.Common.DTOs;
using HotelBookings.Domain.Entities;
using HotelBookings.Domain.Interfaces;
using HotelBookings.Domain.Specifications;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HotelBookings.Application.Services
{
    public class HotelService(IGenericRepository<Hotel> hotelRepository,
                              IGenericRepository<Booking> bookingRepository,
                              IGenericRepository<Room> roomRepository)
                : IHotelService
    {
        public async Task<IReadOnlyList<HotelBaseDto>> SearchHotelsAsync(SearchRequestDto searchRequest)
        {
            Expression<Func<Hotel, bool>>? searchExpression = PredicateBuilder.New<Hotel>(true);

            if (!string.IsNullOrWhiteSpace(searchRequest.Name))
            {
                var searchTerm = $"%{searchRequest.Name.ToLower()}%";

                searchExpression = searchExpression.And(h => EF.Functions.Like(h.Name.ToLower(), searchTerm));
            }

            BaseSpecification<Hotel> hotelSearchSpec = new(searchExpression);

            if (searchRequest.PageIndex.HasValue && searchRequest.PageSize.HasValue)
            {
                int resultsToSkip =  searchRequest.PageIndex.Value * searchRequest.PageSize.Value;

                hotelSearchSpec.ApplyPagging(searchRequest.PageSize.Value, resultsToSkip);
            }

            hotelSearchSpec.AddOrderBy(h => h.Name);

            var hotels = await hotelRepository.GetAllAsync(hotelSearchSpec);

            return hotels.ToHotelBaseDtoList();
        }

        public async Task<IReadOnlyList<RoomDto>> GetAvailableRoomsAsync(AvailabilityRequestDto availabilityRequest)
        {
            BookedRoomsSpecification bookedRoomsSpec = new(availabilityRequest);

            var bookedRoomIds = bookingRepository.ExpressionToQuery(bookedRoomsSpec.Criteria).Select(b => b.RoomId);

            Expression<Func<Room, bool>>? availableRoomsExpression = PredicateBuilder.New<Room>(true);

            if (availabilityRequest.HotelId != null) {
                availableRoomsExpression = r => r.HotelId.Equals(availabilityRequest.HotelId);
            }

            availableRoomsExpression = availableRoomsExpression.And(
                                        r => r.Capacity >= availabilityRequest.Guests &&
                                            !bookedRoomIds.Contains(r.Id)
            );

            BaseSpecification<Room> roomSearchSpec = new(availableRoomsExpression);

            roomSearchSpec.AddInclude(r => r.Hotel);

            var rooms = await roomRepository.GetAllAsync(roomSearchSpec);

            return rooms.ToRoomDtoList();
        }

        public async Task<HotelDto?> GetHotelAsync(Guid id)
        {
            var hotel = await hotelRepository.GetByIdAsync(id);
            return hotel?.ToHotelDto();
        }

        public async Task<IReadOnlyList<RoomDto>?> GetHotelRoomsAsync(Guid hotelId)
        {
            BaseSpecification<Room> roomSpec = new(r => r.HotelId.Equals(hotelId));

            roomSpec.AddInclude(r => r.Hotel);

            roomSpec.AddOrderBy(r => r.Type);

            var rooms = await roomRepository.GetAllAsync(roomSpec);

            return rooms.ToRoomDtoList();
        }
    }
}
