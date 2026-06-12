using MediatR;
using TicketingSystem.Services.DTOs.TicketDtos;

public record GetTicketDetailsQuery(
    Guid TicketId
) : IRequest<TicketDto?>;