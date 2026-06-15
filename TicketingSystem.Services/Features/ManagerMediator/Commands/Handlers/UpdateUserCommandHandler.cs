using AutoMapper;
using MediatR;
using TicketingSystem.Repository.UnitOfWork.Abstraction;
using TicketingSystem.Services.Features.ManagerMediator.Contracts;

namespace TicketingSystem.Services.Features.ManagerMediator.Commands
{
    public class UpdateUserCommandHandler
        : IRequestHandler<UpdateUserCommand, UserDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserDto?> Handle(
            UpdateUserCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.Id);

            if (user == null)
                return null;

            _mapper.Map(request.Dto, user);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }
    }
}
