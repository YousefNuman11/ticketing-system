using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketingSystem.API.Features.Manager.Commands.UpdateUser;
using TicketingSystem.API.Features.Manager.Queries.GetAllTickets;
using TicketingSystem.API.Features.Manager.Queries.GetClientsWithTickets;
using TicketingSystem.Services.DTOs.TicketDtos;
using TicketingSystem.Services.DTOs.User;
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

        // Register Employee
        [HttpPost("employee")]
        public async Task<IActionResult> CreateEmployee(CreateEmployeeDto dto)
        {
            var result = await _mediator.Send(
                new CreateEmployeeCommand(dto));

            return Ok(result);
        }

        //Get a List of Employees
        [HttpGet("employees")]
        public async Task<IActionResult> GetEmployees([FromQuery] PaginationDto pagination)
        {
            var result = await _mediator.Send(
                new GetEmployeesQuery(pagination));
            return Ok(result);
        }

        // Get a list of client
        [HttpGet("clients")]
        public async Task<IActionResult> GetClients([FromQuery] PaginationDto pagination)
        {
            var result = await _mediator.Send(
                new GetClientsQuery(pagination));
            return Ok(result);
        }

        //Get a client by Id
        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var user = await _mediator.Send(
               new GetUserByIdQuery(id));
            return user == null ? NotFound() : Ok(user);
        }

        // Toggle user status
        [HttpPut("users/{id}/toggle-status")]
        public async Task<IActionResult> Toggle(Guid id)
        {
            var user = await _mediator.Send(
                new ToggleUserStatusCommand(id));
            return user == null ? NotFound() : Ok(user);
        }

        // Update the users info
        [HttpPut("users/{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateUserDto dto)
        {
            var user = await _mediator.Send(
               new UpdateUserCommand(
                    id,
                    dto));
            return user == null ? NotFound() : Ok(user);
        }

        // Get a List of Clients with their tickets
        [Authorize(Roles = "Manager")]
        [HttpGet("clients-with-tickets")]
        public async Task<IActionResult> GetClientsWithTickets([FromQuery] PaginationDto pagination)
        {
            var result = await _mediator.Send(
                new GetClientsWithTicketsQuery(pagination));
            return Ok(result);
        }

        //Assign ticket to spec. employee
        [Authorize(Roles = "Manager")]
        [HttpPut("{ticketId}/assign/{employeeId}")]
        public async Task<IActionResult> Assign(Guid ticketId, Guid employeeId)
        {
            await _mediator.Send(
                new AssignTicketCommand(
                    ticketId,
                    employeeId));
            return Ok(new { message = "Ticket assigned successfully" });
        }

        // List all tickets with status / employees/ external clients 
        [Authorize(Roles = "Manager")]
        [HttpGet("all")]
        public async Task<IActionResult> GetTickets(
            [FromQuery] TicketFilterDto filter,
            [FromQuery] PaginationDto pagination)
        {
            var result = await _mediator.Send(
                new GetAllTicketsQuery(
                    filter,
                    pagination));
            return Ok(result);
        }

        //view ticket information
        [Authorize(Roles = "Manager")]
        [HttpGet("info/{id}")]
        public async Task<IActionResult> GetTicket(Guid id)
        {
            return Ok(await _mediator.Send(
                new GetTicketDetailsQuery(id)));
        }
    }
}