using MediatR;
using TicketingSystem.Services.DTOs.UserDtos;
using TicketingSystem.Services.Helpers;

namespace TicketingSystem.API.Features.Manager.Queries.GetClientsWithTickets
{
    public record GetClientsWithTicketsQuery(
        PaginationDto Pagination
    ) : IRequest<PagedResult<ClientWithTicketsDto>>;
}