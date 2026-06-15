using MediatR;

namespace TicketingSystem.Services.Features.DashboardMediator.Queries
{
    public record GetTicketTrendQuery : IRequest<object>;
}
