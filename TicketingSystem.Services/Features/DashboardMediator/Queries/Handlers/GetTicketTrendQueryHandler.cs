using MediatR;
using Microsoft.EntityFrameworkCore;
using TicketingSystem.Repository.Specifications.DashBoard;
using TicketingSystem.Repository.Specifications.Dashboard;
using TicketingSystem.Repository.UnitOfWork.Abstraction;

namespace TicketingSystem.Services.Features.DashboardMediator.Queries
{
    public class GetTicketTrendQueryHandler
        : IRequestHandler<GetTicketTrendQuery, object>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTicketTrendQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<object> Handle(
            GetTicketTrendQuery request,
            CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Tickets.Query(new AllTicketsSpec());

            return await TicketTrendSpec.Apply(query).ToListAsync(cancellationToken);
        }
    }
}
