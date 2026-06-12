using MediatR;
using TicketingSystem.Services.DTOs.TicketDtos;
using TicketingSystem.Services.Helpers;

namespace TicketingSystem.API.Features.Tickets.Queries.GetMyTickets
{
    public class GetMyTicketsQuery
        : IRequest<PagedResult<TicketDto>>
    {
        public Guid ClientId { get; set; }
        public PaginationDto Pagination { get; set; }

        public GetMyTicketsQuery(
            Guid clientId,
            PaginationDto pagination)
        {
            ClientId = clientId;
            Pagination = pagination;
        }
    }
}