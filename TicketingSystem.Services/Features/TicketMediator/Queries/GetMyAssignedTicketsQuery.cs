using MediatR;
using TicketingSystem.Services.DTOs.TicketDtos;
using TicketingSystem.Services.Helpers;

public record GetMyAssignedTicketsQuery(
    Guid EmployeeId,
    PaginationDto Pagination
) : IRequest<PagedResult<TicketDto>>;