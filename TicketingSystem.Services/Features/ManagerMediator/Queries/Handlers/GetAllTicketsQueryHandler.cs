using AutoMapper;
using MediatR;
using TicketingSystem.Repository.Models;
using TicketingSystem.Repository.UnitOfWork.Abstraction;
using TicketingSystem.Services.Features.TicketMediator.Contracts;
using TicketingSystem.Services.Helpers;

namespace TicketingSystem.Services.Features.ManagerMediator.Queries
{
    public class GetAllTicketsQueryHandler
        : IRequestHandler<GetAllTicketsQuery, PagedResult<TicketDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllTicketsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResult<TicketDto>> Handle(
            GetAllTicketsQuery request,
            CancellationToken cancellationToken)
        {
            var filter = request.Filter;

            IQueryable<Ticket> query = _unitOfWork.Tickets.QueryTickets();

            if (!string.IsNullOrWhiteSpace(filter.Status) &&
                Enum.TryParse<TicketStatus>(filter.Status, out var status))
            {
                query = query.Where(t => t.Status == status);
            }

            if (filter.EmployeeId.HasValue)
            {
                query = query.Where(t => t.AssignedEmployeeId == filter.EmployeeId.Value);
            }

            if (filter.ClientId.HasValue)
            {
                query = query.Where(t => t.UserId == filter.ClientId.Value);
            }

            return await PaginationHelper
                .ToPagedResultAsync<Ticket, TicketDto>(query, request.Pagination, _mapper);
        }
    }
}
