using AutoMapper;
using MediatR;
using TicketingSystem.Repository.Models;
using TicketingSystem.Repository.UnitOfWork.Abstraction;
using TicketingSystem.Services.Features.TicketMediator.Contracts;
using TicketingSystem.Services.Exceptions;

namespace TicketingSystem.Services.Features.TicketMediator.Commands
{
    public class CreateTicketCommandHandler
        : IRequestHandler<CreateTicketCommand, TicketDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateTicketCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TicketDto> Handle(
            CreateTicketCommand request,
            CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(request.Dto.ProductId);

            if (product == null)
                throw new NotFoundException("Product not found");

            var ticket = _mapper.Map<Ticket>(request.Dto);

            ticket.Id = Guid.NewGuid();
            ticket.UserId = request.ClientId;
            ticket.Status = TicketStatus.New;
            ticket.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.Tickets.AddAsync(ticket);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<TicketDto>(ticket);
        }
    }
}
