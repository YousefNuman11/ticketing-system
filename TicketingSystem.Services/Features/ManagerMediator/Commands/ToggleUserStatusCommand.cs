using MediatR;
using TicketingSystem.Services.DTOs.User;

public record ToggleUserStatusCommand(
    Guid Id
) : IRequest<UserDto?>;