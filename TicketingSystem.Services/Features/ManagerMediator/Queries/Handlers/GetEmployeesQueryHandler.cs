using AutoMapper;
using MediatR;
using TicketingSystem.Repository.Models;
using TicketingSystem.Repository.UnitOfWork.Abstraction;
using TicketingSystem.Services.DTOs.User;
using TicketingSystem.Services.Helpers;

public class GetEmployeesQueryHandler
    : IRequestHandler<GetEmployeesQuery, PagedResult<UserDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetEmployeesQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedResult<UserDto>> Handle(
        GetEmployeesQuery request,
        CancellationToken cancellationToken)
    {
        var query = _unitOfWork.Users
            .GetUsersQuery()
            .Where(u => u.Role == UserRole.Employee);

        return await PaginationHelper
            .ToPagedResultAsync<User, UserDto>(
                query,
                request.Pagination,
                _mapper);
    }
}