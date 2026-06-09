using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TicketingSystem.Services.DTOs;
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
    }
}