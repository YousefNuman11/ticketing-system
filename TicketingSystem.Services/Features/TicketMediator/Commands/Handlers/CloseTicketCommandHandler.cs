using MediatR;
using TicketingSystem.Repository.Models;
using TicketingSystem.Repository.UnitOfWork.Abstraction;

public class CloseTicketCommandHandler
    : IRequestHandler<CloseTicketCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public CloseTicketCommandHandler(
        IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        CloseTicketCommand request,
        CancellationToken cancellationToken)
    {
        var ticket = await _unitOfWork.Tickets
            .GetByIdAsync(request.TicketId);

        if (ticket == null)
            throw new Exception("Ticket not found");

        if (ticket.UserId != request.ClientId)
            throw new Exception("Not your ticket");

        if (ticket.Status != TicketStatus.Resolved)
            throw new Exception("Must be resolved first");

        ticket.Status = TicketStatus.Closed;

        await _unitOfWork.SaveChangesAsync();
    }
}