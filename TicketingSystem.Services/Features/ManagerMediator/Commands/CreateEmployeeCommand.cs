using MediatR;
using TicketingSystem.Services.Features.ManagerMediator.Contracts;

namespace TicketingSystem.Services.Features.ManagerMediator.Commands
{
    public record CreateEmployeeCommand(CreateEmployeeDto Dto) : IRequest<UserDto>;
}
