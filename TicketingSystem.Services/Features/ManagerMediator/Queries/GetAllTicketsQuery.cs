using MediatR;
using TicketingSystem.Services.Features.TicketMediator.Contracts;
using TicketingSystem.Services.Helpers;

namespace TicketingSystem.Services.Features.ManagerMediator.Queries
{
    public record GetAllTicketsQuery(TicketFilterDto Filter, PaginationDto Pagination)
        : IRequest<PagedResult<TicketDto>>;
}
