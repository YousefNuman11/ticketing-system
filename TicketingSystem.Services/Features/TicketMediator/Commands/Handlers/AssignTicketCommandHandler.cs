using MediatR;
using TicketingSystem.Repository.Models;
using TicketingSystem.Repository.UnitOfWork.Abstraction;

public class AssignTicketCommandHandler
    : IRequestHandler<AssignTicketCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public AssignTicketCommandHandler(
        IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        AssignTicketCommand request,
        CancellationToken cancellationToken)
    {
        var ticket = await _unitOfWork.Tickets
            .GetByIdAsync(request.TicketId);

        if (ticket == null)
            throw new Exception("Ticket not found");

        if (ticket.AssignedEmployeeId != null)
            throw new Exception("Already assigned");

        var employee = await _unitOfWork.Users
            .GetByIdAsync(request.EmployeeId);

        if (employee == null ||
            employee.Role != UserRole.Employee)
        {
            throw new Exception("Invalid employee");
        }

        ticket.AssignedEmployeeId = request.EmployeeId;
        ticket.Status = TicketStatus.InProgress;

        await _unitOfWork.SaveChangesAsync();
    }
}