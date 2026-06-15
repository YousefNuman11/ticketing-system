using MediatR;
using Microsoft.AspNetCore.Http;
using TicketingSystem.Services.Features.TicketMediator.Contracts;

namespace TicketingSystem.Services.Features.TicketMediator.Commands
{
    public record UploadAttachmentCommand(Guid TicketId, Guid UserId, IFormFile File)
        : IRequest<AttachmentDto>;
}
