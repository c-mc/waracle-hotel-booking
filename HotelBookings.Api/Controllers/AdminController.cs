using HotelBookings.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookings.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController(IAdminService adminService) : ControllerBase
    {
        [HttpPost("data")]
        [ProducesResponseType(201)]
        [ProducesResponseType(409)]
        [EndpointSummary("Seeds the database with data")]
        [EndpointDescription("Call this endpoint to seed the database with hotel and room data. Seeding is only possible when the database is empty.")]
        public async Task<IActionResult> SeedDatabase()
        {
            var seedingOutcome = await adminService.SeedDatabaseAsync();

            return seedingOutcome.StatusCode switch
            {
                System.Net.HttpStatusCode.Created => Created(),
                System.Net.HttpStatusCode.Conflict => Conflict(seedingOutcome.Message),
                _ => throw new ArgumentException($"Unexpected seedingOutcome StatusCode of {seedingOutcome.StatusCode} returned"),
            };
        }

        [HttpDelete("data")]
        [ProducesResponseType(200)]
        [ProducesResponseType(409)]
        [EndpointSummary("Resets the database, removing all data")]
        [EndpointDescription("Call this endpoint to empty the database, removing all data from it.")]
        public IActionResult RemoveDatabaseData()
        {
            var dataRemoved = adminService.RemoveDatabaseDataAsync();

            return dataRemoved.StatusCode == System.Net.HttpStatusCode.OK ? Ok() : Conflict(dataRemoved.Message);
        }
    }
}
