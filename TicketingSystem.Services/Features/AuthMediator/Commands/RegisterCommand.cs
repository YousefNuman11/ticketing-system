using MediatR;
using TicketingSystem.Services.Features.AuthMediator.Contracts;

namespace TicketingSystem.Services.Features.AuthMediator.Commands
{
    public record RegisterCommand(RegisterDto Dto) : IRequest<AuthResponseDto>;
}
