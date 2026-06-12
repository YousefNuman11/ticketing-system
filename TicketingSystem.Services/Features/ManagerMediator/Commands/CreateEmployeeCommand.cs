using MediatR;
using TicketingSystem.Services.DTOs.User;

public record CreateEmployeeCommand(
    CreateEmployeeDto Dto
) : IRequest<UserDto>;