using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TicketingSystem.Repository.UnitOfWork.Abstraction;
using TicketingSystem.Services.DTOs.TicketDtos;

public class GetTicketByIdQueryHandler
    : IRequestHandler<GetTicketByIdQuery, TicketDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetTicketByIdQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TicketDto?> Handle(
        GetTicketByIdQuery request,
        CancellationToken cancellationToken)
    {
        var ticket = await _unitOfWork.Tickets
            .GetByUserId(request.ClientId)
            .FirstOrDefaultAsync(t => t.Id == request.TicketId);

        return ticket == null
            ? null
            : _mapper.Map<TicketDto>(ticket);
    }
}