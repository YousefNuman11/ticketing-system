using AutoMapper;
using MediatR;
using TicketingSystem.Repository.UnitOfWork.Abstraction;
using TicketingSystem.Services.DTOs.User;

public class ToggleUserStatusCommandHandler
    : IRequestHandler<ToggleUserStatusCommand, UserDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ToggleUserStatusCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UserDto?> Handle(
        ToggleUserStatusCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(request.Id);

        if (user == null)
            return null;

        user.IsActive = !user.IsActive;

        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<UserDto>(user);
    }
}