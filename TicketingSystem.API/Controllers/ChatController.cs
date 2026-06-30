using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TicketingSystem.Repository.Models;
using TicketingSystem.Services.Features.ChatMediator.Contract;
using TicketingSystem.Services.Features.ChatMediator.Queries;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ChatController : ControllerBase
{
    private readonly IMediator _mediator;

    public ChatController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    private UserRole CurrentRole =>
        Enum.Parse<UserRole>(User.FindFirst(ClaimTypes.Role)!.Value);

    [HttpPost("ask")]
    public async Task<IActionResult> Ask([FromBody] AskChatbotRequestDto dto)
    {
        var result = await _mediator.Send(
            new AskChatbotQuery(dto.Question, CurrentUserId, CurrentRole));
        return Ok(result);
    }
}