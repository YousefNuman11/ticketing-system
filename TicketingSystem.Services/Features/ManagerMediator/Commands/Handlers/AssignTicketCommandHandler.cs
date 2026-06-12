using MediatR;
using TicketingSystem.Services.Service.Abstraction;

namespace TicketingSystem.API.Features.Manager.Commands.AssignTicket
{
    public class AssignTicketCommandHandler
        : IRequestHandler<AssignTicketCommand, Unit>
    {
        private readonly ITicketService _ticketService;

        public AssignTicketCommandHandler(
            ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        public async Task<Unit> Handle(
            AssignTicketCommand request,
            CancellationToken cancellationToken)
        {
            await _ticketService.AssignTicketAsync(
                request.TicketId,
                request.EmployeeId);

            return Unit.Value;
        }
    }
}