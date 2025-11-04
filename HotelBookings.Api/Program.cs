using HotelBookings.Api.Middleware;
using HotelBookings.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelBookings.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = String.Empty;

            if (builder.Environment.IsDevelopment())
            {
                builder.Configuration.AddEnvironmentVariables().AddJsonFile("appsettings.Development.json");
                connectionString = builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING");
            }
            else
            {
                connectionString = Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRING");
            }

            builder.Services.AddDbContext<HotelBookingContext>(options => options.UseSqlServer(connectionString, b => b.MigrationsAssembly("HotelBookings.Api")));

            builder.Services.Register();

            var app = builder.Build();

            if (!app.Environment.IsProduction())
            {
                app.MapOpenApi();

                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/openapi/v1.json", "Open API v1");
                });
            }
            else
            {
                app.UseExceptionHandler(exceptionHandlerApp
                        => exceptionHandlerApp.Run(async context
                            => await Results.Problem()
                               .ExecuteAsync(context)));
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
