using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Services.Features.ManagerMediator.Commands;
using TicketingSystem.Services.Features.ManagerMediator.Contracts;
using TicketingSystem.Services.Features.ManagerMediator.Queries;
using TicketingSystem.Services.Features.TicketMediator.Contracts;
using TicketingSystem.Services.Helpers;

namespace TicketingSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Manager")]
    public class ManagerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ManagerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Register an employee
        [HttpPost("employee")]
        public async Task<IActionResult> CreateEmployee(CreateEmployeeDto dto)
        {
            var result = await _mediator.Send(new CreateEmployeeCommand(dto));
            return Ok(result);
        }

        // List employees
        [HttpGet("employees")]
        public async Task<IActionResult> GetEmployees([FromQuery] PaginationDto pagination)
        {
            var result = await _mediator.Send(new GetEmployeesQuery(pagination));
            return Ok(result);
        }

        // List clients
        [HttpGet("clients")]
        public async Task<IActionResult> GetClients([FromQuery] PaginationDto pagination)
        {
            var result = await _mediator.Send(new GetClientsQuery(pagination));
            return Ok(result);
        }

        // Get a user by id
        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var user = await _mediator.Send(new GetUserByIdQuery(id));
            return user == null ? NotFound() : Ok(user);
        }

        // Toggle a user's active status
        [HttpPut("users/{id}/toggle-status")]
        public async Task<IActionResult> Toggle(Guid id)
        {
            var user = await _mediator.Send(new ToggleUserStatusCommand(id));
            return user == null ? NotFound() : Ok(user);
        }

        // Update a user's info
        [HttpPut("users/{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateUserDto dto)
        {
            var user = await _mediator.Send(new UpdateUserCommand(id, dto));
            return user == null ? NotFound() : Ok(user);
        }

        // Clients together with their tickets
        [HttpGet("clients-with-tickets")]
        public async Task<IActionResult> GetClientsWithTickets([FromQuery] PaginationDto pagination)
        {
            var result = await _mediator.Send(new GetClientsWithTicketsQuery(pagination));
            return Ok(result);
        }

        // Assign a ticket to a specific employee
        [HttpPut("{ticketId}/assign/{employeeId}")]
        public async Task<IActionResult> Assign(Guid ticketId, Guid employeeId)
        {
            await _mediator.Send(new AssignTicketCommand(ticketId, employeeId));
            return Ok(new { message = "Ticket assigned successfully" });
        }

        // List all tickets, filtered by status / employee / client
        [HttpGet("all")]
        public async Task<IActionResult> GetTickets(
            [FromQuery] TicketFilterDto filter,
            [FromQuery] PaginationDto pagination)
        {
            var result = await _mediator.Send(new GetAllTicketsQuery(filter, pagination));
            return Ok(result);
        }

        // View a single ticket's information
        [HttpGet("info/{id}")]
        public async Task<IActionResult> GetTicket(Guid id)
            => Ok(await _mediator.Send(new GetTicketDetailsQuery(id)));
    }
}
