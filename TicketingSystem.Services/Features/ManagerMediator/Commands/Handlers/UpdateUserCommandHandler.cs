using AutoMapper;
using MediatR;
using TicketingSystem.Repository.Search;
using TicketingSystem.Repository.UnitOfWork.Abstraction;
using TicketingSystem.Services.Features.ManagerMediator.Contracts;

namespace TicketingSystem.Services.Features.ManagerMediator.Commands
{
    public class UpdateUserCommandHandler
        : IRequestHandler<UpdateUserCommand, UserDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserSearchService _searchService;

        public UpdateUserCommandHandler(IUnitOfWork unitOfWork,
            IMapper mapper,
            IUserSearchService searchService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _searchService = searchService;
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

            _searchService.IndexUser(
                user.Id, user.FullName, user.Email, user.Address, user.Role.ToString());

            return _mapper.Map<UserDto>(user);
        }
    }
}
