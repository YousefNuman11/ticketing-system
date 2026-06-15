using MediatR;

namespace TicketingSystem.Services.Features.ManagerMediator.Commands
{
    public record AssignTicketCommand(Guid TicketId, Guid EmployeeId) : IRequest;
}
