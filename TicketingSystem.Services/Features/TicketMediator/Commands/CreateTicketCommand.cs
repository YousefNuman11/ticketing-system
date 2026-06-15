using MediatR;
using TicketingSystem.Services.Features.TicketMediator.Contracts;

namespace TicketingSystem.Services.Features.TicketMediator.Commands
{
    public record CreateTicketCommand(CreateTicketDto Dto, Guid ClientId)
        : IRequest<TicketDto>;
}
