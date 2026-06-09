using TicketingSystem.Services.DTOs.AuthenticationDto;

namespace TicketingSystem.Services.Service.Abstraction
{
    public interface IAuthService
    {
        Task<AuthResponseDto> Register(RegisterDto dto);
        Task<AuthResponseDto> Login(LoginDto dto);
    }
}