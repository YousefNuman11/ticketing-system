using AutoMapper;
using MediatR;
using TicketingSystem.Repository.Models;
using TicketingSystem.Repository.UnitOfWork.Abstraction;
using TicketingSystem.Services.DTOs.TicketDtos;
using TicketingSystem.Services.Helpers;

public class GetMyAssignedTicketsQueryHandler
    : IRequestHandler<GetMyAssignedTicketsQuery,
        PagedResult<TicketDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetMyAssignedTicketsQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedResult<TicketDto>> Handle(
        GetMyAssignedTicketsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _unitOfWork.Tickets
            .QueryTickets()
            .Where(t => t.AssignedEmployeeId == request.EmployeeId);

        return await PaginationHelper
            .ToPagedResultAsync<Ticket, TicketDto>(
                query,
                request.Pagination,
                _mapper);
    }
}