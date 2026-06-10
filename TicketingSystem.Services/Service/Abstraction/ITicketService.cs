using TicketingSystem.Services.DTOs.CommentDtos;
using TicketingSystem.Services.DTOs.TicketDtos;

namespace TicketingSystem.Services.Service.Abstraction
{
    public interface ITicketService
    {
        Task<TicketDto> CreateTicketAsync(CreateTicketDto dto,Guid clientId);
        Task<List<TicketDto>> GetMyTicketsAsync(Guid clientId);
        Task<TicketDto?> GetTicketByIdAsync(Guid ticketId, Guid clientId);
        Task AssignTicketAsync(Guid ticketId, Guid employeeId);
        Task<List<TicketDto>> GetMyAssignedTicketsAsync(Guid employeeId);
        Task<CommentDto> AddCommentAsync(Guid ticketId,Guid userId,AddCommentDto dto);
        Task<List<CommentDto>> GetCommentsAsync(Guid ticketId);

        Task ResolveTicketAsync(Guid ticketId,Guid employeeId);
        Task CloseTicketAsync(Guid ticketId,Guid clientId);
    }
}