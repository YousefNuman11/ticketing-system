using MediatR;

public record ResolveTicketCommand(
    Guid TicketId,
    Guid EmployeeId
) : IRequest;