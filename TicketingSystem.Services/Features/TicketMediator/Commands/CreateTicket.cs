using MediatR;
using TicketingSystem.Services.DTOs.TicketDtos;

namespace TicketingSystem.API.Features.Tickets.Commands.CreateTicket
{
    public class CreateTicketCommand : IRequest<TicketDto>
    {
        public CreateTicketDto Dto { get; set; }
        public Guid ClientId { get; set; }

        public CreateTicketCommand(CreateTicketDto dto, Guid clientId)
        {
            Dto = dto;
            ClientId = clientId;
        }
    }
}