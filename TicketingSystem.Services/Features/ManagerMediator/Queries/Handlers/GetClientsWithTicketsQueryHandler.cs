using AutoMapper;
using MediatR;
using TicketingSystem.Repository.Models;
using TicketingSystem.Repository.Search;
using TicketingSystem.Repository.Specifications.Users;
using TicketingSystem.Repository.UnitOfWork.Abstraction;
using TicketingSystem.Services.Features.ManagerMediator.Contracts;
using TicketingSystem.Services.Features.ManagerMediator.Queries;
using TicketingSystem.Services.Helpers;

public class GetClientsWithTicketsQueryHandler
    : IRequestHandler<GetClientsWithTicketsQuery, PagedResult<ClientWithTicketsDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserSearchService _searchService;

    public GetClientsWithTicketsQueryHandler(
        IUnitOfWork unitOfWork, IMapper mapper, IUserSearchService searchService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _searchService = searchService;
    }

    public async Task<PagedResult<ClientWithTicketsDto>> Handle(
        GetClientsWithTicketsQuery request,
        CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var matchingIds = _searchService.Search(request.Search, roleFilter: "Client");
            var spec = new UsersByIdsWithTicketsSpec(matchingIds, UserRole.Client);
            var query = _unitOfWork.Users.Query(spec);

            return await PaginationHelper
                .ToPagedResultAsync<User, ClientWithTicketsDto>(query, request.Pagination, _mapper);
        }

        var fallbackSpec = new ClientsSpec();
        var fallbackQuery = _unitOfWork.Users.Query(fallbackSpec);

        return await PaginationHelper
            .ToPagedResultAsync<User, ClientWithTicketsDto>(fallbackQuery, request.Pagination, _mapper);
    }
}