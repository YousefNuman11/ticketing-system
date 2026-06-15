using AutoMapper;
using MediatR;
using TicketingSystem.Repository.Models;
using TicketingSystem.Repository.Specifications.Users;
using TicketingSystem.Repository.UnitOfWork.Abstraction;
using TicketingSystem.Services.Features.ManagerMediator.Contracts;
using TicketingSystem.Services.Helpers;

namespace TicketingSystem.Services.Features.ManagerMediator.Queries
{
    public class GetClientsWithTicketsQueryHandler
        : IRequestHandler<GetClientsWithTicketsQuery, PagedResult<ClientWithTicketsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetClientsWithTicketsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResult<ClientWithTicketsDto>> Handle(
            GetClientsWithTicketsQuery request,
            CancellationToken cancellationToken)
        {
            var spec = new ClientsSpec();

            var query = _unitOfWork.Users.Query(spec);

            return await PaginationHelper
                .ToPagedResultAsync<User, ClientWithTicketsDto>(query, request.Pagination, _mapper);
        }
    }
}
