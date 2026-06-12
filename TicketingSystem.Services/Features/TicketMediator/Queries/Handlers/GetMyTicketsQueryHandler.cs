using AutoMapper;
using MediatR;
using TicketingSystem.API.Features.Tickets.Queries.GetMyTickets;
using TicketingSystem.Repository.Models;
using TicketingSystem.Repository.Specifications;
using TicketingSystem.Repository.UnitOfWork.Abstraction;
using TicketingSystem.Services.DTOs.TicketDtos;
using TicketingSystem.Services.Helpers;

namespace TicketingSystem.Services.Features.TicketMediator.Queries.Handler
{
    public class GetMyTicketsQueryHandler
        : IRequestHandler<GetMyTicketsQuery, PagedResult<TicketDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetMyTicketsQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResult<TicketDto>> Handle(
            GetMyTicketsQuery request,
            CancellationToken cancellationToken)
        {
            var spec = new TicketsByUserSpec(request.ClientId);

            var query = _unitOfWork.Tickets.Query(spec);

            return await PaginationHelper
                .ToPagedResultAsync<Ticket, TicketDto>(
                    query,
                    request.Pagination,
                    _mapper);
        }
    }
}