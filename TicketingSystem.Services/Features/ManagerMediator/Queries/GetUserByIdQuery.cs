using MediatR;
using TicketingSystem.Services.Features.ManagerMediator.Contracts;

namespace TicketingSystem.Services.Features.ManagerMediator.Queries
{
    public record GetUserByIdQuery(Guid Id) : IRequest<UserDto?>;
}
