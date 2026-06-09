using TicketingSystem.Services.DTOs;

namespace TicketingSystem.Services.Service.Abstraction
{
    public interface IAuthService
    {
        Task<AuthResponseDto> Register(RegisterDto dto);
        Task<AuthResponseDto> Login(LoginDto dto);
    }
}