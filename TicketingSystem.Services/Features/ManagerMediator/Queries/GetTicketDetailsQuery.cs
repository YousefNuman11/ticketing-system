using MediatR;
using TicketingSystem.Services.DTOs.TicketDtos;

namespace TicketingSystem.API.Features.Manager.Queries.GetTicketDetails
{
    public record GetTicketDetailsQuery(
        Guid TicketId
    ) : IRequest<TicketDto?>;
}