using MediatR;
using TicketingSystem.Services.DTOs.TicketDtos;
using TicketingSystem.Services.Service.Abstraction;

namespace TicketingSystem.API.Features.Manager.Queries.GetTicketDetails
{
    public class GetTicketDetailsQueryHandler
        : IRequestHandler<GetTicketDetailsQuery, TicketDto?>
    {
        private readonly ITicketService _ticketService;

        public GetTicketDetailsQueryHandler(
            ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        public async Task<TicketDto?> Handle(
            GetTicketDetailsQuery request,
            CancellationToken cancellationToken)
        {
            return await _ticketService
                .GetTicketDetailsAsync(request.TicketId);
        }
    }
}