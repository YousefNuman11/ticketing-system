using MediatR;

namespace TicketingSystem.Services.Features.TicketMediator.Commands
{
    public record ResolveTicketCommand(Guid TicketId, Guid EmployeeId) : IRequest;
}
