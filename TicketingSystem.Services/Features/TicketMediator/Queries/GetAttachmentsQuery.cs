using MediatR;
using TicketingSystem.Services.DTOs.TicketAttachmentDto;
using TicketingSystem.Services.Helpers;

public record GetAttachmentsQuery(
    Guid TicketId,
    PaginationDto Pagination
) : IRequest<PagedResult<AttachmentDto>>;