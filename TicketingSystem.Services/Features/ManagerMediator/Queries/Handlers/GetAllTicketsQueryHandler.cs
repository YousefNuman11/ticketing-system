using MediatR;
using TicketingSystem.Services.DTOs.TicketDtos;
using TicketingSystem.Services.Helpers;
using TicketingSystem.Services.Service.Abstraction;

namespace TicketingSystem.API.Features.Manager.Queries.GetAllTickets
{
    public class GetAllTicketsQueryHandler
        : IRequestHandler<GetAllTicketsQuery,
            PagedResult<TicketDto>>
    {
        private readonly ITicketService _ticketService;

        public GetAllTicketsQueryHandler(
            ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        public async Task<PagedResult<TicketDto>> Handle(
            GetAllTicketsQuery request,
            CancellationToken cancellationToken)
        {
            return await _ticketService
                .GetAllTicketsAsync(
                    request.Filter,
                    request.Pagination);
        }
    }
}