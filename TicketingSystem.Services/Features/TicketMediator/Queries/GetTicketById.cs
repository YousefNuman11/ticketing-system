using MediatR;
using TicketingSystem.Services.DTOs.TicketDtos;

public record GetTicketByIdQuery(
    Guid TicketId,
    Guid ClientId
) : IRequest<TicketDto?>;