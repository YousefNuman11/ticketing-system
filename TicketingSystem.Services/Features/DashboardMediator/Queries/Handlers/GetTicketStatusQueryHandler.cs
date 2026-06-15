using MediatR;
using Microsoft.EntityFrameworkCore;
using TicketingSystem.Repository.Specifications.DashBoard;
using TicketingSystem.Repository.Specifications.Dashboard;
using TicketingSystem.Repository.UnitOfWork.Abstraction;

namespace TicketingSystem.Services.Features.DashboardMediator.Queries
{
    public class GetTicketStatusQueryHandler
        : IRequestHandler<GetTicketStatusQuery, object>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTicketStatusQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<object> Handle(
            GetTicketStatusQuery request,
            CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Tickets.Query(new AllTicketsSpec());

            return await TicketStatusSpec.Apply(query).ToListAsync(cancellationToken);
        }
    }
}
