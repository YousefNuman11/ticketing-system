using MediatR;
using TicketingSystem.Services.DTOs.TicketDtos;

public record UpdateTicketCommand(
    Guid TicketId,
    Guid ClientId,
    UpdateTicketDto Dto
) : IRequest<TicketDto?>;