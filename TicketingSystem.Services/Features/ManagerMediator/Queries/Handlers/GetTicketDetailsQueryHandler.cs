using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TicketingSystem.Repository.Specifications;
using TicketingSystem.Repository.UnitOfWork.Abstraction;
using TicketingSystem.Services.Features.TicketMediator.Contracts;

namespace TicketingSystem.Services.Features.ManagerMediator.Queries
{
    public class GetTicketDetailsQueryHandler
        : IRequestHandler<GetTicketDetailsQuery, TicketDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetTicketDetailsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TicketDto?> Handle(
            GetTicketDetailsQuery request,
            CancellationToken cancellationToken)
        {
            var spec = new TicketDetailsSpec(request.TicketId);

            var ticket = await _unitOfWork.Tickets.Query(spec)
                .FirstOrDefaultAsync(cancellationToken);

            return ticket == null ? null : _mapper.Map<TicketDto>(ticket);
        }
    }
}
