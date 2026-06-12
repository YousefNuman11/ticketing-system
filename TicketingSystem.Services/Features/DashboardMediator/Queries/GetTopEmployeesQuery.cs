using MediatR;
using TicketingSystem.Services.Service.Abstraction;

namespace TicketingSystem.API.Features.Dashboard.Queries.GetTopEmployees
{
    public class GetTopEmployeesQueryHandler
        : IRequestHandler<GetTopEmployeesQuery, object>
    {
        private readonly IDashboardService _dashboardService;

        public GetTopEmployeesQueryHandler(
            IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public async Task<object> Handle(
            GetTopEmployeesQuery request,
            CancellationToken cancellationToken)
        {
            return await _dashboardService.GetTopEmployeesAsync();
        }
    }
}