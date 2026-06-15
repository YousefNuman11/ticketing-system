using MediatR;
using TicketingSystem.Repository.Models;
using TicketingSystem.Repository.UnitOfWork.Abstraction;
using TicketingSystem.Services.Exceptions;

namespace TicketingSystem.Services.Features.TicketMediator.Commands
{
    public class ResolveTicketCommandHandler
        : IRequestHandler<ResolveTicketCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ResolveTicketCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(
            ResolveTicketCommand request,
            CancellationToken cancellationToken)
        {
            var ticket = await _unitOfWork.Tickets.GetByIdAsync(request.TicketId);

            if (ticket == null)
                throw new NotFoundException("Ticket not found");

            if (ticket.AssignedEmployeeId != request.EmployeeId)
                throw new ForbiddenException("Not assigned to you");

            ticket.Status = TicketStatus.Resolved;

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
