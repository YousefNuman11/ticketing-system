using MediatR;
using TicketingSystem.Services.Features.TicketMediator.Contracts;

namespace TicketingSystem.Services.Features.TicketMediator.Commands
{
    public record UpdateTicketCommand(Guid TicketId, Guid ClientId, UpdateTicketDto Dto)
        : IRequest<TicketDto?>;
}
