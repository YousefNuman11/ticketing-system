using MediatR;
using TicketingSystem.Services.Features.TicketMediator.Contracts;
using TicketingSystem.Services.Helpers;

namespace TicketingSystem.Services.Features.TicketMediator.Queries
{
    public record GetAttachmentsQuery(Guid TicketId, PaginationDto Pagination)
        : IRequest<PagedResult<AttachmentDto>>;
}
