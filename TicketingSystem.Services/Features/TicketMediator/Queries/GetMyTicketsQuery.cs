using MediatR;
using TicketingSystem.Services.Features.TicketMediator.Contracts;
using TicketingSystem.Services.Helpers;

namespace TicketingSystem.Services.Features.TicketMediator.Queries
{
    public record GetMyTicketsQuery(Guid ClientId, PaginationDto Pagination, string? Search)
        : IRequest<PagedResult<TicketDto>>;
}
