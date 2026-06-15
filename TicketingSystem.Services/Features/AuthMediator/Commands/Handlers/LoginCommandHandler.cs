using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using TicketingSystem.Repository.UnitOfWork.Abstraction;
using TicketingSystem.Services.Features.AuthMediator.Contracts;
using TicketingSystem.Services.Features.ManagerMediator.Contracts;
using TicketingSystem.Services.Settings;
using TicketingSystem.Services.Exceptions;

namespace TicketingSystem.Services.Features.AuthMediator.Commands
{
    public class LoginCommandHandler
        : IRequestHandler<LoginCommand, AuthResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly JwtSettings _jwtSettings;

        public LoginCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IOptions<JwtSettings> options)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _jwtSettings = options.Value;
        }

        public async Task<AuthResponseDto> Handle(
            LoginCommand request,
            CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            var users = await _unitOfWork.Users.GetAllAsync();

            var user = users.FirstOrDefault(x =>
                x.Email == dto.Identifier ||
                x.MobileNumber == dto.Identifier);

            if (user == null || !user.IsActive)
                throw new UnauthorizedException("Invalid credentials");

            var valid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

            if (!valid)
                throw new UnauthorizedException("Invalid credentials");

            var token = JwtTokenGenerator.Generate(user, _jwtSettings);

            return new AuthResponseDto
            {
                Token = token,
                User = _mapper.Map<UserDto>(user)
            };
        }
    }
}
