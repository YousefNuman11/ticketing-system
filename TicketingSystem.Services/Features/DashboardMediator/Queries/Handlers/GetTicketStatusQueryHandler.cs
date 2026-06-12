using MediatR;
using TicketingSystem.Services.Service.Abstraction;

namespace TicketingSystem.API.Features.Dashboard.Queries.GetTicketStatus
{
    public class GetTicketStatusQueryHandler
        : IRequestHandler<GetTicketStatusQuery, object>
    {
        private readonly IDashboardService _dashboardService;

        public GetTicketStatusQueryHandler(
            IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public async Task<object> Handle(
            GetTicketStatusQuery request,
            CancellationToken cancellationToken)
        {
            return await _dashboardService.GetTicketStatusAsync();
        }
    }
}