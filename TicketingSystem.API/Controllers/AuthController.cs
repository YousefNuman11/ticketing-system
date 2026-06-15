using MediatR;
using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Services.Features.AuthMediator.Commands;
using TicketingSystem.Services.Features.AuthMediator.Contracts;

namespace TicketingSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Client register
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
            => Ok(await _mediator.Send(new RegisterCommand(dto)));

        // Client and Employee login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
            => Ok(await _mediator.Send(new LoginCommand(dto)));
    }
}
