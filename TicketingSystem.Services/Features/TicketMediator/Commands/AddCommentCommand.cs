using MediatR;
using TicketingSystem.Services.Features.TicketMediator.Contracts;

namespace TicketingSystem.Services.Features.TicketMediator.Commands
{
    public record AddCommentCommand(Guid TicketId, Guid UserId, AddCommentDto Dto)
        : IRequest<CommentDto>;
}
