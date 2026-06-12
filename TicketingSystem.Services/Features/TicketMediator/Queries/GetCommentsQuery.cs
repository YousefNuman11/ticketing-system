using MediatR;
using TicketingSystem.Services.DTOs.CommentDtos;
using TicketingSystem.Services.Helpers;

public record GetCommentsQuery(
    Guid TicketId,
    PaginationDto Pagination
) : IRequest<PagedResult<CommentDto>>;