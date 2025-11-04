namespace HotelBookings.Common.DTOs
{
    public class SearchRequestDto : PaginationDto
    {
        public string? Name { get; set; }
    }
}
