using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TicketingSystem.Repository.Models;
using TicketingSystem.Repository.UnitOfWork.Abstraction;
using TicketingSystem.Services.DTOs.AuthenticationDto;
using TicketingSystem.Services.DTOs.User;
using TicketingSystem.Services.Service.Abstraction;
using TicketingSystem.Services.Settings;

namespace TicketingSystem.Services.Service
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly JwtSettings _jwtSettings;

        public AuthService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IOptions<JwtSettings> options)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _jwtSettings = options.Value;
        }

        public async Task<AuthResponseDto> Register(RegisterDto dto)
        {
            var users = await _unitOfWork.Users.GetAllAsync();

            var exists = users.Any(x => x.Email == dto.Email);

            if (exists)
                throw new Exception("Email already exists");

            var user = _mapper.Map<User>(dto);

            user.Id = Guid.NewGuid();
            user.Role = UserRole.Client;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            user.IsActive = true;

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();


            var token = GenerateToken(user);

            return new AuthResponseDto
            {
                Token = token,
                User = _mapper.Map<UserDto>(user)
            };
        }


        public async Task<AuthResponseDto> Login(LoginDto dto)
        {
            var users = await _unitOfWork.Users.GetAllAsync();

            var user = users.FirstOrDefault(x =>
                x.Email == dto.Identifier ||
                x.MobileNumber == dto.Identifier);

            if (user == null || !user.IsActive)
                throw new Exception("Invalid credentials");

            var valid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

            if (!valid)
                throw new Exception("Invalid credentials");

            var token = GenerateToken(user);

            return new AuthResponseDto
            {
                Token = token,
                User = _mapper.Map<UserDto>(user)
            };
        }

        private string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.Key));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(_jwtSettings.ExpiryInDays),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}