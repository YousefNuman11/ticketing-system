using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TicketingSystem.Services.DTOs.CommentDtos;
using TicketingSystem.Services.DTOs.TicketDtos;
using TicketingSystem.Services.Helpers;
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

        //List of all tickets related to a spec. client
        [Authorize(Roles = "Client")]
        [HttpGet("myTickets")]
        public async Task<IActionResult> GetMyTickets([FromQuery] PaginationDto pagination)
        {
            var clientId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await _ticketService.GetMyTicketsAsync(clientId, pagination);

            return Ok(result);
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

        //Client can add new ticket
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

        //Modify a ticket if its status is Open
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

        // Employees will see a list of assigned tickets
        [Authorize(Roles = "Employee")]
        [HttpGet("assigned")]
        public async Task<IActionResult> GetAssignedTickets([FromQuery] PaginationDto pagination)
        {
            var employeeId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await _ticketService.GetMyAssignedTicketsAsync(employeeId, pagination);

            return Ok(result);
        }

        // Get comments for spec. ticket
        [HttpGet("{ticketId}/comments")]
        public async Task<IActionResult> GetComments(
            Guid ticketId,
            [FromQuery] PaginationDto pagination)
        {
            var result = await _ticketService.GetCommentsAsync(ticketId, pagination);

            return Ok(result);
        }

        //The client and employee can see the comments and reply to each other
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

        //The employee will resolve the ticket.
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

        //The client can close it if everything is ok.
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

        //Get Attachment
        [HttpGet("{ticketId}/attachments")]
        public async Task<IActionResult> GetAttachments(
            Guid ticketId,
            [FromQuery] PaginationDto pagination)
        {
            var result = await _ticketService.GetAttachmentsAsync(ticketId, pagination);

            return Ok(result);
        }

        //Upload Attachment
        [HttpPost("{ticketId}/attachments")]
        public async Task<IActionResult> UploadAttachment(
            Guid ticketId,
            IFormFile file)
        {
            var userId = Guid.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await _ticketService
                .UploadAttachmentAsync(ticketId, userId, file);

            return Ok(result);
        }
    }
}