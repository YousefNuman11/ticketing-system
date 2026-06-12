using AutoMapper;
using MediatR;
using TicketingSystem.Repository.UnitOfWork.Abstraction;
using TicketingSystem.Services.DTOs.TicketDtos;

public class GetTicketDetailsQueryHandler
    : IRequestHandler<GetTicketDetailsQuery, TicketDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetTicketDetailsQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TicketDto?> Handle(
        GetTicketDetailsQuery request,
        CancellationToken cancellationToken)
    {
        var ticket = await _unitOfWork.Tickets
            .GetTicketWithDetailsAsync(request.TicketId);

        return ticket == null
            ? null
            : _mapper.Map<TicketDto>(ticket);
    }
}