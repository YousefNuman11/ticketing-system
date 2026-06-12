using MediatR;
using TicketingSystem.Services.DTOs.User;

namespace TicketingSystem.API.Features.Manager.Commands.UpdateUser
{
    public record UpdateUserCommand(
        Guid Id,
        UpdateUserDto Dto
    ) : IRequest<UserDto?>;
}