using AutoMapper;
using MediatR;
using TicketingSystem.Repository.Models;
using TicketingSystem.Repository.UnitOfWork.Abstraction;
using TicketingSystem.Services.DTOs.User;
using TicketingSystem.Services.Helpers;

public class GetClientsQueryHandler
    : IRequestHandler<GetClientsQuery, PagedResult<UserDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetClientsQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedResult<UserDto>> Handle(
        GetClientsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _unitOfWork.Users
            .GetUsersQuery()
            .Where(u => u.Role == UserRole.Client);

        return await PaginationHelper
            .ToPagedResultAsync<User, UserDto>(
                query,
                request.Pagination,
                _mapper);
    }
}