using MediatR;
using TicketingSystem.Services.Features.ManagerMediator.Contracts;
using TicketingSystem.Services.Helpers;

namespace TicketingSystem.Services.Features.ManagerMediator.Queries
{
    public record GetClientsWithTicketsQuery(PaginationDto Pagination)
        : IRequest<PagedResult<ClientWithTicketsDto>>;
}
