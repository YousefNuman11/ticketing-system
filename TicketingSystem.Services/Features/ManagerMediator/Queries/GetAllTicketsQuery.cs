using MediatR;
using TicketingSystem.Services.DTOs.TicketDtos;
using TicketingSystem.Services.Helpers;

namespace TicketingSystem.API.Features.Manager.Queries.GetAllTickets
{
    public record GetAllTicketsQuery(
        TicketFilterDto Filter,
        PaginationDto Pagination
    ) : IRequest<PagedResult<TicketDto>>;
}