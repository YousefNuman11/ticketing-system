using MediatR;
using TicketingSystem.Services.Features.ManagerMediator.Contracts;
using TicketingSystem.Services.Helpers;

namespace TicketingSystem.Services.Features.ManagerMediator.Queries
{
    public record GetClientsQuery(PaginationDto Pagination)
        : IRequest<PagedResult<UserDto>>;
}
