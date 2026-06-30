using AutoMapper;
using MediatR;
using TicketingSystem.Repository.Models;
using TicketingSystem.Repository.Specifications;
using TicketingSystem.Repository.UnitOfWork.Abstraction;
using TicketingSystem.Services.Features.TicketMediator.Contracts;
using TicketingSystem.Services.Helpers;

namespace TicketingSystem.Services.Features.TicketMediator.Queries
{
    public class GetMyTicketsQueryHandler
        : IRequestHandler<GetMyTicketsQuery, PagedResult<TicketDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetMyTicketsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResult<TicketDto>> Handle(
            GetMyTicketsQuery request,
            CancellationToken cancellationToken)
        {
            var spec = new TicketsByUserSpec(request.ClientId, request.Search);

            var query = _unitOfWork.Tickets.Query(spec);

            return await PaginationHelper
                .ToPagedResultAsync<Ticket, TicketDto>(query, request.Pagination, _mapper);
        }
    }
}
