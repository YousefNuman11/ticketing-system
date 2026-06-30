using AutoMapper;
using MediatR;
using TicketingSystem.Repository.Models;
using TicketingSystem.Repository.Search;
using TicketingSystem.Repository.UnitOfWork.Abstraction;
using TicketingSystem.Services.Features.ManagerMediator.Contracts;

namespace TicketingSystem.Services.Features.ManagerMediator.Commands
{
    public class CreateEmployeeCommandHandler
        : IRequestHandler<CreateEmployeeCommand, UserDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserSearchService _searchService;

        public CreateEmployeeCommandHandler(IUnitOfWork unitOfWork,
            IMapper mapper,
            IUserSearchService searchService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _searchService = searchService;
        }

        public async Task<UserDto> Handle(
            CreateEmployeeCommand request,
            CancellationToken cancellationToken)
        {
            var user = _mapper.Map<User>(request.Dto);

            user.Id = Guid.NewGuid();
            user.Role = UserRole.Employee;
            user.IsActive = true;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Dto.Password);

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            _searchService.IndexUser(
                user.Id, user.FullName, user.Email, user.Address, user.Role.ToString());

            return _mapper.Map<UserDto>(user);
        }
    }
}
