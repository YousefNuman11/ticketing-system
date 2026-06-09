using TicketingSystem.Services.DTOs.TicketDtos;

namespace TicketingSystem.Services.Service.Abstraction
{
    public interface ITicketService
    {
        Task<TicketDto> CreateTicketAsync(
            CreateTicketDto dto,
            Guid clientId);
    }
}