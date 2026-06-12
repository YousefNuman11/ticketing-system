using MediatR;
using TicketingSystem.Services.DTOs.CommentDtos;

public record AddCommentCommand(
    Guid TicketId,
    Guid UserId,
    AddCommentDto Dto
) : IRequest<CommentDto>;