using MediatR;
using TicketingSystem.Repository.Models;
using TicketingSystem.Services.Features.ChatMediator.Contract;

namespace TicketingSystem.Services.Features.ChatMediator.Queries
{
    public record AskChatbotQuery(string Question, Guid UserId, UserRole Role)
        : IRequest<ChatResponseDto>;
}