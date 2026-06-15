using MediatR;
using TicketingSystem.Services.Features.ManagerMediator.Contracts;

namespace TicketingSystem.Services.Features.ManagerMediator.Commands
{
    public record UpdateUserCommand(Guid Id, UpdateUserDto Dto) : IRequest<UserDto?>;
}
