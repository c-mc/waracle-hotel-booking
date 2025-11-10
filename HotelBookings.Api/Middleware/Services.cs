using HotelBookings.Application.Services;
using HotelBookings.Domain.Entities;
using HotelBookings.Domain.Interfaces;
using HotelBookings.Infrastructure.Repositories;

namespace HotelBookings.Api.Middleware
{
    public static class Services
    {
        public static void Register(this IServiceCollection services) {

            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddControllers();
            
            services.AddOpenApi();

            services.AddScoped<IGenericRepository<Hotel>, GenericRepository<Hotel>>();
            services.AddScoped<IGenericRepository<Room>, GenericRepository<Room>>();
            services.AddScoped<IBookingRepository, BookingRepository>();

            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IHotelService, HotelService>();
            services.AddScoped<IAdminService, AdminService>();
        }
    }
}
