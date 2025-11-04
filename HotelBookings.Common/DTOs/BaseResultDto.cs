using System.Net;

namespace HotelBookings.Common.DTOs
{
    public class BaseResultDto
    {
        public HttpStatusCode? StatusCode { get; set; }

        public string? Message { get; set; }
    }
}
