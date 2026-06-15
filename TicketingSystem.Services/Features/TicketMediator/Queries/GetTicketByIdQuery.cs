using MediatR;
using TicketingSystem.Services.Features.TicketMediator.Contracts;

namespace TicketingSystem.Services.Features.TicketMediator.Queries
{
    public record GetTicketByIdQuery(Guid TicketId, Guid ClientId)
        : IRequest<TicketDto?>;
}
