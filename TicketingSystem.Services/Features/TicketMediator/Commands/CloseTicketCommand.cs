using MediatR;

public record CloseTicketCommand(
    Guid TicketId,
    Guid ClientId
) : IRequest;