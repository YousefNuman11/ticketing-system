using MediatR;
using TicketingSystem.Services.Features.ManagerMediator.Contracts;

namespace TicketingSystem.Services.Features.ManagerMediator.Commands
{
    public record ToggleUserStatusCommand(Guid Id) : IRequest<UserDto?>;
}
