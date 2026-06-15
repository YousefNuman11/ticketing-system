using MediatR;

namespace TicketingSystem.Services.Features.TicketMediator.Commands
{
    public record CloseTicketCommand(Guid TicketId, Guid ClientId) : IRequest;
}
