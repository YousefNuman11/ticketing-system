using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using TicketingSystem.Repository.Models;
using TicketingSystem.Repository.UnitOfWork.Abstraction;
using TicketingSystem.Services.Features.AuthMediator.Contracts;
using TicketingSystem.Services.Features.ManagerMediator.Contracts;
using TicketingSystem.Services.Settings;
using TicketingSystem.Services.Exceptions;

namespace TicketingSystem.Services.Features.AuthMediator.Commands
{
    public class RegisterCommandHandler
        : IRequestHandler<RegisterCommand, AuthResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly JwtSettings _jwtSettings;

        public RegisterCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IOptions<JwtSettings> options)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _jwtSettings = options.Value;
        }

        public async Task<AuthResponseDto> Handle(
            RegisterCommand request,
            CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            var users = await _unitOfWork.Users.GetAllAsync();

            if (users.Any(x => x.Email == dto.Email))
                throw new ValidationException("Email already exists");

            var user = _mapper.Map<User>(dto);

            user.Id = Guid.NewGuid();
            user.Role = UserRole.Client; // Only the manager can create employees
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            user.IsActive = true;

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            var token = JwtTokenGenerator.Generate(user, _jwtSettings);

            return new AuthResponseDto
            {
                Token = token,
                User = _mapper.Map<UserDto>(user)
            };
        }
    }
}
