using MediatR;
using TicketingSystem.Services.DTOs.UserDtos;
using TicketingSystem.Services.Helpers;
using TicketingSystem.Services.Service.Abstraction;

namespace TicketingSystem.API.Features.Manager.Queries.GetClientsWithTickets
{
    public class GetClientsWithTicketsQueryHandler
        : IRequestHandler<GetClientsWithTicketsQuery,
            PagedResult<ClientWithTicketsDto>>
    {
        private readonly IManagerService _managerService;

        public GetClientsWithTicketsQueryHandler(
            IManagerService managerService)
        {
            _managerService = managerService;
        }

        public async Task<PagedResult<ClientWithTicketsDto>> Handle(
            GetClientsWithTicketsQuery request,
            CancellationToken cancellationToken)
        {
            return await _managerService
                .GetClientsWithTicketsAsync(request.Pagination);
        }
    }
}