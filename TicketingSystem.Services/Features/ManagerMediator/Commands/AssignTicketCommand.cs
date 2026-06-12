using MediatR;

namespace TicketingSystem.API.Features.Manager.Commands.AssignTicket
{
    public record AssignTicketCommand(
        Guid TicketId,
        Guid EmployeeId
    ) : IRequest<Unit>;
}