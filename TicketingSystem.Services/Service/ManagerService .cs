using AutoMapper;
using TicketingSystem.Repository.Models;
using TicketingSystem.Repository.UnitOfWork.Abstraction;
using TicketingSystem.Services.DTOs.User;
using TicketingSystem.Services.Service.Abstraction;

namespace TicketingSystem.Services.Service
{
    public class ManagerService : IManagerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ManagerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserDto> CreateEmployeeAsync(CreateEmployeeDto dto)
        {
            var user = _mapper.Map<User>(dto);

            user.Id = Guid.NewGuid();
            user.Role = UserRole.Employee;
            user.IsActive = true;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task<List<UserDto>> GetEmployeesAsync()
        {
            var users = await _unitOfWork.Users.GetAllAsync();

            var filtered = users.Where(x => x.Role == UserRole.Employee);

            return _mapper.Map<List<UserDto>>(filtered);
        }

        public async Task<List<UserDto>> GetClientsAsync()
        {
            var users = await _unitOfWork.Users.GetAllAsync();

            var filtered = users.Where(x => x.Role == UserRole.Client);

            return _mapper.Map<List<UserDto>>(filtered);
        }

        public async Task<UserDto?> GetUserByIdAsync(Guid id)
        {
            var users = await _unitOfWork.Users.GetAllAsync();

            var user = users.FirstOrDefault(x => x.Id == id);

            return user == null ? null : _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto?> ToggleUserStatusAsync(Guid id)
        {
            var users = await _unitOfWork.Users.GetAllAsync();

            var user = users.FirstOrDefault(x => x.Id == id);

            if (user == null) return null;

            user.IsActive = !user.IsActive;

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto?> UpdateUserAsync(Guid id, UpdateUserDto dto)
        {
            var users = await _unitOfWork.Users.GetAllAsync();

            var user = users.FirstOrDefault(x => x.Id == id);

            if (user == null) return null;

            _mapper.Map(dto, user); 

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }
    }
}
