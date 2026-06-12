using MediatR;
using TicketingSystem.Services.DTOs.User;

public record GetUserByIdQuery(
    Guid Id
) : IRequest<UserDto?>;