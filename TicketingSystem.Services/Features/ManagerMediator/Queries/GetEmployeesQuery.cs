using MediatR;
using TicketingSystem.Services.DTOs.User;
using TicketingSystem.Services.Helpers;

public record GetEmployeesQuery(
    PaginationDto Pagination
) : IRequest<PagedResult<UserDto>>;