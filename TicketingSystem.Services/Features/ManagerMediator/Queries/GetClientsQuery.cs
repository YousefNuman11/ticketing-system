using MediatR;
using TicketingSystem.Services.DTOs.User;
using TicketingSystem.Services.Helpers;

public record GetClientsQuery(
    PaginationDto Pagination
) : IRequest<PagedResult<UserDto>>;