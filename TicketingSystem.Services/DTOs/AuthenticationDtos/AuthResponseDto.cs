using TicketingSystem.Services.DTOs.User;

namespace TicketingSystem.Services.DTOs.AuthenticationDto
{
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public UserDto User { get; set; }
    }
}