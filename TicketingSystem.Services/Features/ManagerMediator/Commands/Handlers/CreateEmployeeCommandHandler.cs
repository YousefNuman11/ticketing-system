using AutoMapper;
using MediatR;
using TicketingSystem.Repository.Models;
using TicketingSystem.Repository.UnitOfWork.Abstraction;
using TicketingSystem.Services.DTOs.User;

public class CreateEmployeeCommandHandler
    : IRequestHandler<CreateEmployeeCommand, UserDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateEmployeeCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(
        CreateEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        var user = _mapper.Map<User>(request.Dto);

        user.Id = Guid.NewGuid();
        user.Role = UserRole.Employee;
        user.IsActive = true;
        user.PasswordHash =
            BCrypt.Net.BCrypt.HashPassword(request.Dto.Password);

        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<UserDto>(user);
    }
}