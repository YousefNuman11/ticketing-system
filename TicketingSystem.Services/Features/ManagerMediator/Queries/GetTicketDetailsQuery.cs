using MediatR;
using TicketingSystem.Services.Features.TicketMediator.Contracts;

namespace TicketingSystem.Services.Features.ManagerMediator.Queries
{
    public record GetTicketDetailsQuery(Guid TicketId) : IRequest<TicketDto?>;
}
