using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TicketingSystem.Repository.Models;
using TicketingSystem.Repository.Specifications.Users;
using TicketingSystem.Repository.UnitOfWork.Abstraction;
using TicketingSystem.Services.DTOs.User;
using TicketingSystem.Services.DTOs.UserDtos;
using TicketingSystem.Services.Helpers;
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

        //CREATE EMPLOYEE
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

        //GET EMPLOYEES
        public async Task<PagedResult<UserDto>> GetEmployeesAsync(PaginationDto pagination)
        {
            var spec = new EmployeesSpec();

            var query = _unitOfWork.Users.Query(spec);

            return await PaginationHelper
                .ToPagedResultAsync<User, UserDto>(query, pagination, _mapper);
        }

        // GET CLIENTS 
        public async Task<PagedResult<UserDto>> GetClientsAsync(PaginationDto pagination)
        {
            var spec = new ClientsSpec();

            var query = _unitOfWork.Users.Query(spec);

            return await PaginationHelper
                .ToPagedResultAsync<User, UserDto>(query, pagination, _mapper);
        }

        // GET USER BY ID 
        public async Task<UserDto?> GetUserByIdAsync(Guid id)
        {
            var spec = new UserByIdSpec(id);

            var user = await _unitOfWork.Users.Query(spec)
                .FirstOrDefaultAsync();

            return user == null ? null : _mapper.Map<UserDto>(user); ;
        }

        //  TOGGLE STATUS
        public async Task<UserDto?> ToggleUserStatusAsync(Guid id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);

            if (user == null) return null;

            user.IsActive = !user.IsActive;

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        //UPDATE USER
        public async Task<UserDto?> UpdateUserAsync(Guid id, UpdateUserDto dto)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);

            if (user == null) return null;

            _mapper.Map(dto, user);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        //CLIENTS WITH TICKETS
        public async Task<PagedResult<ClientWithTicketsDto>> GetClientsWithTicketsAsync(
            PaginationDto pagination)
        {
            var spec = new ClientsSpec();

            var query = _unitOfWork.Users.Query(spec);

            return await PaginationHelper
                .ToPagedResultAsync<User, ClientWithTicketsDto>(query, pagination, _mapper);
        }
    }
}