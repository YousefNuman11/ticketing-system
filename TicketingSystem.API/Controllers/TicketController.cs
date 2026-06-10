using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TicketingSystem.Services.DTOs;
using TicketingSystem.Services.DTOs.CommentDtos;
using TicketingSystem.Services.DTOs.TicketDtos;
using TicketingSystem.Services.Service.Abstraction;

namespace TicketingSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketController(
            ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [Authorize(Roles ="Client")]
        [HttpGet("myTickets")]
        public async Task<IActionResult> GetMyTickets()
        {
            var clientId = Guid.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            return Ok(await _ticketService.GetMyTicketsAsync(clientId));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var clientId = Guid.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await _ticketService.GetTicketByIdAsync(id, clientId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> CreateTicket(
           [FromBody] CreateTicketDto dto)
        {
            var userId = Guid.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!
                    .Value);

            var result =
                await _ticketService.CreateTicketAsync(
                    dto,
                    userId);

            return Ok(result);
        }

        [Authorize(Roles = "Client")]
        [HttpPut("{ticketId}")]
        public async Task<IActionResult> UpdateTicket(
            Guid ticketId,
            UpdateTicketDto dto)
        {
            var clientId = Guid.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await _ticketService.UpdateTicketAsync(
                ticketId,
                clientId,
                dto);

            return result == null
                ? NotFound()
                : Ok(result);
        }

        [Authorize(Roles = "Employee")]
        [HttpGet("assigned")]
        public async Task<IActionResult> GetAssignedTickets()
        {
            var employeeId = Guid.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await _ticketService.GetMyAssignedTicketsAsync(employeeId);

            return Ok(result);
        }

        [Authorize(Roles = "Manager")]
        [HttpPut("{ticketId}/assign/{employeeId}")]
        public async Task<IActionResult> Assign(Guid ticketId, Guid employeeId)
        {
            await _ticketService.AssignTicketAsync(ticketId, employeeId);
            return Ok(new { message = "Ticket assigned successfully" });
        }

        [HttpGet("{ticketId}/comments")]
        public async Task<IActionResult> GetComments(
            Guid ticketId)
        {
            return Ok(
                await _ticketService.GetCommentsAsync(ticketId));
        }

        [Authorize(Roles = "Client,Employee")]
        [HttpPost("{ticketId}/comments")]
        public async Task<IActionResult> AddComment(
            Guid ticketId,
            [FromBody] AddCommentDto dto)
        {
            var userId = Guid.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await _ticketService.AddCommentAsync(
                ticketId,
                userId,
                dto);

            return Ok(result);
        }

        [Authorize(Roles = "Employee")]
        [HttpPut("{ticketId}/resolve")]
        public async Task<IActionResult> ResolveTicket(Guid ticketId)
        {
            var employeeId = Guid.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            await _ticketService.ResolveTicketAsync(
                ticketId,
                employeeId);

            return Ok(new
            {
                Message = "Ticket resolved successfully"
            });
        }

        [Authorize(Roles = "Client")]
        [HttpPut("{ticketId}/close")]
        public async Task<IActionResult> CloseTicket(Guid ticketId)
        {
            var clientId = Guid.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            await _ticketService.CloseTicketAsync(
                ticketId,
                clientId);

            return Ok(new
            {
                Message = "Ticket closed successfully"
            });
        }

        [Authorize(Roles = "Manager")]
        [HttpGet("all")]
        public async Task<IActionResult> GetTickets([FromQuery] TicketFilterDto filter)
        {
            return Ok(await _ticketService.GetAllTicketsAsync(filter));
        }

        [Authorize(Roles = "Manager")]
        [HttpGet("info/{id}")]
        public async Task<IActionResult> GetTicket(Guid id)
        {
            return Ok(await _ticketService.GetTicketDetailsAsync(id));
        }
    }
}