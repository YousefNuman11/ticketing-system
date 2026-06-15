using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TicketingSystem.Services.Features.TicketMediator.Commands;
using TicketingSystem.Services.Features.TicketMediator.Contracts;
using TicketingSystem.Services.Features.TicketMediator.Queries;
using TicketingSystem.Services.Helpers;

namespace TicketingSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TicketController(IMediator mediator)
        {
            _mediator = mediator;
        }

        private Guid CurrentUserId =>
            Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        // All tickets for the current client
        [Authorize(Roles = "Client")]
        [HttpGet("myTickets")]
        public async Task<IActionResult> GetMyTickets([FromQuery] PaginationDto pagination)
        {
            var result = await _mediator.Send(
                new GetMyTicketsQuery(CurrentUserId, pagination));
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(
                new GetTicketByIdQuery(id, CurrentUserId));

            return result == null ? NotFound() : Ok(result);
        }

        // Client creates a new ticket
        [HttpPost]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> CreateTicket([FromBody] CreateTicketDto dto)
        {
            var result = await _mediator.Send(
                new CreateTicketCommand(dto, CurrentUserId));
            return Ok(result);
        }

        // Modify a ticket while its status is still New
        [Authorize(Roles = "Client")]
        [HttpPut("{ticketId}")]
        public async Task<IActionResult> UpdateTicket(Guid ticketId, UpdateTicketDto dto)
        {
            var result = await _mediator.Send(
                new UpdateTicketCommand(ticketId, CurrentUserId, dto));

            return result == null ? NotFound() : Ok(result);
        }

        // Employees see the tickets assigned to them
        [Authorize(Roles = "Employee")]
        [HttpGet("assigned")]
        public async Task<IActionResult> GetAssignedTickets([FromQuery] PaginationDto pagination)
        {
            var result = await _mediator.Send(
                new GetMyAssignedTicketsQuery(CurrentUserId, pagination));
            return Ok(result);
        }

        // Comments for a specific ticket
        [HttpGet("{ticketId}/comments")]
        public async Task<IActionResult> GetComments(Guid ticketId, [FromQuery] PaginationDto pagination)
        {
            var result = await _mediator.Send(
                new GetCommentsQuery(ticketId, pagination));
            return Ok(result);
        }

        // Client and employee reply to each other
        [Authorize(Roles = "Client,Employee")]
        [HttpPost("{ticketId}/comments")]
        public async Task<IActionResult> AddComment(Guid ticketId, [FromBody] AddCommentDto dto)
        {
            var result = await _mediator.Send(
                new AddCommentCommand(ticketId, CurrentUserId, dto));
            return Ok(result);
        }

        // Employee resolves the ticket
        [Authorize(Roles = "Employee")]
        [HttpPut("{ticketId}/resolve")]
        public async Task<IActionResult> ResolveTicket(Guid ticketId)
        {
            await _mediator.Send(new ResolveTicketCommand(ticketId, CurrentUserId));
            return Ok(new { Message = "Ticket resolved successfully" });
        }

        // Client closes the ticket once satisfied
        [Authorize(Roles = "Client")]
        [HttpPut("{ticketId}/close")]
        public async Task<IActionResult> CloseTicket(Guid ticketId)
        {
            await _mediator.Send(new CloseTicketCommand(ticketId, CurrentUserId));
            return Ok(new { Message = "Ticket closed successfully" });
        }

        // Attachments for a ticket
        [HttpGet("{ticketId}/attachments")]
        public async Task<IActionResult> GetAttachments(Guid ticketId, [FromQuery] PaginationDto pagination)
        {
            var result = await _mediator.Send(
                new GetAttachmentsQuery(ticketId, pagination));
            return Ok(result);
        }

        // Upload an attachment
        [HttpPost("{ticketId}/attachments")]
        public async Task<IActionResult> UploadAttachment(Guid ticketId, IFormFile file)
        {
            var result = await _mediator.Send(
                new UploadAttachmentCommand(ticketId, CurrentUserId, file));
            return Ok(result);
        }
    }
}
