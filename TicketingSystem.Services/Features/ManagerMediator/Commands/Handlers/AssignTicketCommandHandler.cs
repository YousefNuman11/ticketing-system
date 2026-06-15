using MediatR;
using TicketingSystem.Repository.Models;
using TicketingSystem.Repository.UnitOfWork.Abstraction;
using TicketingSystem.Services.Exceptions;

namespace TicketingSystem.Services.Features.ManagerMediator.Commands
{
    public class AssignTicketCommandHandler
        : IRequestHandler<AssignTicketCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AssignTicketCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(
            AssignTicketCommand request,
            CancellationToken cancellationToken)
        {
            var ticket = await _unitOfWork.Tickets.GetByIdAsync(request.TicketId);

            if (ticket == null)
                throw new NotFoundException("Ticket not found");

            if (ticket.AssignedEmployeeId != null)
                throw new ValidationException("Already assigned");

            var employee = await _unitOfWork.Users.GetByIdAsync(request.EmployeeId);

            if (employee == null || employee.Role != UserRole.Employee)
                throw new ValidationException("Invalid employee");

            ticket.AssignedEmployeeId = request.EmployeeId;
            ticket.Status = TicketStatus.InProgress;

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
