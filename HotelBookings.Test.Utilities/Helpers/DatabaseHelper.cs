using HotelBookings.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelBookings.Test.Utilities.Helpers
{
    public static class DatabaseHelper
    {
        public static DbContextOptions<HotelBookingContext> ContextOptions()
        {
            var connectionString = Environment.GetEnvironmentVariable("DOCKER_SQL_CONNECTIONSTRING");
            return new DbContextOptionsBuilder<HotelBookingContext>().UseSqlServer(connectionString).Options;
        }
    }
}
