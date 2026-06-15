using MediatR;
using TicketingSystem.Services.Features.TicketMediator.Contracts;
using TicketingSystem.Services.Helpers;

namespace TicketingSystem.Services.Features.TicketMediator.Queries
{
    public record GetMyAssignedTicketsQuery(Guid EmployeeId, PaginationDto Pagination)
        : IRequest<PagedResult<TicketDto>>;
}
