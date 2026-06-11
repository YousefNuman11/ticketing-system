using Microsoft.AspNetCore.Http;
using TicketingSystem.Services.DTOs.CommentDtos;
using TicketingSystem.Services.DTOs.TicketAttachmentDto;
using TicketingSystem.Services.DTOs.TicketDtos;
using TicketingSystem.Services.Helpers;

namespace TicketingSystem.Services.Service.Abstraction
{
    public interface ITicketService
    {
        Task<TicketDto> CreateTicketAsync(CreateTicketDto dto, Guid clientId);

        Task<TicketDto?> UpdateTicketAsync(Guid ticketId, Guid clientId, UpdateTicketDto dto);

        Task<PagedResult<TicketDto>> GetMyTicketsAsync(Guid clientId, PaginationDto pagination);

        Task<TicketDto?> GetTicketByIdAsync(Guid ticketId, Guid clientId);

        Task AssignTicketAsync(Guid ticketId, Guid employeeId);

        Task<PagedResult<TicketDto>> GetMyAssignedTicketsAsync(Guid employeeId, PaginationDto pagination);

        Task<CommentDto> AddCommentAsync(Guid ticketId, Guid userId, AddCommentDto dto);

        Task<PagedResult<CommentDto>> GetCommentsAsync(Guid ticketId, PaginationDto pagination);

        Task ResolveTicketAsync(Guid ticketId, Guid employeeId);

        Task CloseTicketAsync(Guid ticketId, Guid clientId);

        Task<PagedResult<TicketDto>> GetAllTicketsAsync(TicketFilterDto filter, PaginationDto pagination);

        Task<TicketDto?> GetTicketDetailsAsync(Guid ticketId);
        Task<AttachmentDto> UploadAttachmentAsync(Guid ticketId,Guid userId,IFormFile file);
        Task<PagedResult<AttachmentDto>> GetAttachmentsAsync(Guid ticketId, PaginationDto pagination);
    }
}