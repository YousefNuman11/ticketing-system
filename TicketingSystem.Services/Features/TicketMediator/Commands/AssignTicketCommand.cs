using MediatR;

public record AssignTicketCommand(
    Guid TicketId,
    Guid EmployeeId
) : IRequest;