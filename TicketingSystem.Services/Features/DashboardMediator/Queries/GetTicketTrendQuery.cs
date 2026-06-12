using MediatR;

namespace TicketingSystem.API.Features.Dashboard.Queries.GetTicketTrend
{
    public record GetTicketTrendQuery : IRequest<object>;
}