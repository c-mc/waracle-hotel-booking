using HotelBookings.Common.DTOs;

namespace HotelBookings.Domain.Interfaces
{
    public interface IAdminService
    {
        Task<BaseResultDto> SeedDatabaseAsync();

        BaseResultDto RemoveDatabaseDataAsync();
    }
}
