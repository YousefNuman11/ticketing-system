using MediatR;
using Microsoft.AspNetCore.Http;
using TicketingSystem.Services.DTOs.TicketAttachmentDto;

public record UploadAttachmentCommand(
    Guid TicketId,
    Guid UserId,
    IFormFile File
) : IRequest<AttachmentDto>;