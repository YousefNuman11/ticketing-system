using TicketingSystem.Services.DTOs.User;
using TicketingSystem.Services.DTOs.UserDtos;
using TicketingSystem.Services.Helpers;

namespace TicketingSystem.Services.Service.Abstraction
{
    public interface IManagerService
    {
        Task<PagedResult<UserDto>> GetEmployeesAsync(PaginationDto pagination);

        Task<PagedResult<UserDto>> GetClientsAsync(PaginationDto pagination);

        Task<UserDto?> GetUserByIdAsync(Guid id);

        Task<UserDto?> ToggleUserStatusAsync(Guid id);

        Task<UserDto?> UpdateUserAsync(Guid id, UpdateUserDto dto);

        Task<UserDto> CreateEmployeeAsync(CreateEmployeeDto dto);

        Task<PagedResult<ClientWithTicketsDto>> GetClientsWithTicketsAsync(PaginationDto pagination);
    }
}