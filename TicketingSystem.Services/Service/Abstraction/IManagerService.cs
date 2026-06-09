using TicketingSystem.Repository.Models;
using TicketingSystem.Services.DTOs;

namespace TicketingSystem.Services.Service.Abstraction
{
    public interface IManagerService
    {
        Task<List<UserDto>> GetEmployeesAsync();
        Task<List<UserDto>> GetClientsAsync();
        Task<UserDto?> GetUserByIdAsync(Guid id);
        Task<UserDto?> ToggleUserStatusAsync(Guid id);
        Task<UserDto?> UpdateUserAsync(Guid id, UpdateUserDto dto);

        Task<UserDto> CreateEmployeeAsync(CreateEmployeeDto dto);
    }
}