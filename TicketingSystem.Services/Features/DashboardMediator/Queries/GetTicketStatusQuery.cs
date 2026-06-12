using MediatR;

namespace TicketingSystem.API.Features.Dashboard.Queries.GetTicketStatus
{
    public record GetTicketStatusQuery : IRequest<object>;
}