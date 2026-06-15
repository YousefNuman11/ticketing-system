using MediatR;
using Microsoft.EntityFrameworkCore;
using TicketingSystem.Repository.Specifications.DashBoard;
using TicketingSystem.Repository.Specifications.Dashboard;
using TicketingSystem.Repository.UnitOfWork.Abstraction;

namespace TicketingSystem.Services.Features.DashboardMediator.Queries
{
    public class GetTopEmployeesQueryHandler
        : IRequestHandler<GetTopEmployeesQuery, object>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTopEmployeesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<object> Handle(
            GetTopEmployeesQuery request,
            CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Tickets.Query(new AllTicketsSpec());

            return await TopEmployeesSpec.Apply(query).ToListAsync(cancellationToken);
        }
    }
}
