using MediatR;
using TicketingSystem.Services.DTOs.User;
using TicketingSystem.Services.Service.Abstraction;

namespace TicketingSystem.API.Features.Manager.Commands.UpdateUser
{
    public class UpdateUserCommandHandler
        : IRequestHandler<UpdateUserCommand, UserDto?>
    {
        private readonly IManagerService _managerService;

        public UpdateUserCommandHandler(
            IManagerService managerService)
        {
            _managerService = managerService;
        }

        public async Task<UserDto?> Handle(
            UpdateUserCommand request,
            CancellationToken cancellationToken)
        {
            return await _managerService
                .UpdateUserAsync(
                    request.Id,
                    request.Dto);
        }
    }
}