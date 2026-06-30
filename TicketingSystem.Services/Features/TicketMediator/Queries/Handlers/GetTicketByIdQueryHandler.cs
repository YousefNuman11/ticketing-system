using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TicketingSystem.Repository.UnitOfWork.Abstraction;
using TicketingSystem.Services.Features.TicketMediator.Contracts;

namespace TicketingSystem.Services.Features.TicketMediator.Queries
{
    public class GetTicketByIdQueryHandler
        : IRequestHandler<GetTicketByIdQuery, TicketDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetTicketByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TicketDto?> Handle(
            GetTicketByIdQuery request,
            CancellationToken cancellationToken)
        {
            var ticket = await _unitOfWork.Tickets
                .QueryTickets()
                .FirstOrDefaultAsync(t => t.Id == request.TicketId, cancellationToken);

            if (ticket == null)
                return null;

            var isOwner = ticket.UserId == request.ClientId;
            var isAssignedEmployee = ticket.AssignedEmployeeId == request.ClientId;

            if (!isOwner && !isAssignedEmployee)
                return null;

            return _mapper.Map<TicketDto>(ticket);
        }
    }
}