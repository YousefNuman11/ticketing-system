using MediatR;
using TicketingSystem.Services.Service.Abstraction;

namespace TicketingSystem.API.Features.Dashboard.Queries.GetTicketTrend
{
    public class GetTicketTrendQueryHandler
        : IRequestHandler<GetTicketTrendQuery, object>
    {
        private readonly IDashboardService _dashboardService;

        public GetTicketTrendQueryHandler(
            IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public async Task<object> Handle(
            GetTicketTrendQuery request,
            CancellationToken cancellationToken)
        {
            return await _dashboardService.GetTicketTrendAsync();
        }
    }
}