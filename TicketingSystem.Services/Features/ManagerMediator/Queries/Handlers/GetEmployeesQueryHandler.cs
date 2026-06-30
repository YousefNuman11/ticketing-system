using AutoMapper;
using MediatR;
using TicketingSystem.Repository.Models;
using TicketingSystem.Repository.Search;
using TicketingSystem.Repository.Specifications.Users;
using TicketingSystem.Repository.UnitOfWork.Abstraction;
using TicketingSystem.Services.Features.ManagerMediator.Contracts;
using TicketingSystem.Services.Features.ManagerMediator.Queries;
using TicketingSystem.Services.Helpers;

public class GetEmployeesQueryHandler
    : IRequestHandler<GetEmployeesQuery, PagedResult<UserDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserSearchService _searchService;

    public GetEmployeesQueryHandler(
        IUnitOfWork unitOfWork, IMapper mapper, IUserSearchService searchService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _searchService = searchService;
    }

    public async Task<PagedResult<UserDto>> Handle(
        GetEmployeesQuery request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var matchingIds = _searchService.Search(request.Search, roleFilter: "Employee");

            var spec = new UsersByIdsSpec(matchingIds, UserRole.Employee);
            Console.WriteLine($"Lucene matched {matchingIds.Count} ids for '{request.Search}'");
            var query = _unitOfWork.Users.Query(spec);

            return await PaginationHelper
                .ToPagedResultAsync<User, UserDto>(query, request.Pagination, _mapper);
        }

        var fallbackSpec = new EmployeesSpec();
        var fallbackQuery = _unitOfWork.Users.Query(fallbackSpec);

        return await PaginationHelper
            .ToPagedResultAsync<User, UserDto>(fallbackQuery, request.Pagination, _mapper);
    }
}