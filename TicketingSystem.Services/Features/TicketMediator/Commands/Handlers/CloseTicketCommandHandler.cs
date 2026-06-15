using MediatR;
using TicketingSystem.Repository.Models;
using TicketingSystem.Repository.UnitOfWork.Abstraction;
using TicketingSystem.Services.Exceptions;

namespace TicketingSystem.Services.Features.TicketMediator.Commands
{
    public class CloseTicketCommandHandler
        : IRequestHandler<CloseTicketCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CloseTicketCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(
            CloseTicketCommand request,
            CancellationToken cancellationToken)
        {
            var ticket = await _unitOfWork.Tickets.GetByIdAsync(request.TicketId);

            if (ticket == null)
                throw new NotFoundException("Ticket not found");

            if (ticket.UserId != request.ClientId)
                throw new ForbiddenException("Not your ticket");

            if (ticket.Status != TicketStatus.Resolved)
                throw new ValidationException("Must be resolved first");

            ticket.Status = TicketStatus.Closed;

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
