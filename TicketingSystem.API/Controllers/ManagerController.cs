using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Services.DTOs.TicketDtos;
using TicketingSystem.Services.DTOs.User;
using TicketingSystem.Services.Helpers;
using TicketingSystem.Services.Service;
using TicketingSystem.Services.Service.Abstraction;

namespace TicketingSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Manager")]
    public class ManagerController : ControllerBase
    {
        private readonly IManagerService _service;
        private readonly ITicketService _ticketService;

        public ManagerController(IManagerService service,
            ITicketService ticketService)
        {
            _service = service;
            _ticketService = ticketService;
        }

        // Register Employee
        [HttpPost("employee")]
        public async Task<IActionResult> CreateEmployee(CreateEmployeeDto dto)
        {
            var result = await _service.CreateEmployeeAsync(dto);
            return Ok(result);
        }

        //Get a List of Employees
        [HttpGet("employees")]
        public async Task<IActionResult> GetEmployees([FromQuery] PaginationDto pagination)
        {
            var result = await _service.GetEmployeesAsync(pagination);
            return Ok(result);
        }

        // Get a list of client
        [HttpGet("clients")]
        public async Task<IActionResult> GetClients([FromQuery] PaginationDto pagination)
        {
            var result = await _service.GetClientsAsync(pagination);
            return Ok(result);
        }

        //Get a client by Id
        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var user = await _service.GetUserByIdAsync(id);
            return user == null ? NotFound() : Ok(user);
        }

        // Toggle user status
        [HttpPut("users/{id}/toggle-status")]
        public async Task<IActionResult> Toggle(Guid id)
        {
            var user = await _service.ToggleUserStatusAsync(id);
            return user == null ? NotFound() : Ok(user);
        }

        // Update the users info
        [HttpPut("users/{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateUserDto dto)
        {
            var user = await _service.UpdateUserAsync(id, dto);
            return user == null ? NotFound() : Ok(user);
        }

        // Get a List of Clients with their tickets
        [Authorize(Roles = "Manager")]
        [HttpGet("clients-with-tickets")]
        public async Task<IActionResult> GetClientsWithTickets([FromQuery] PaginationDto pagination)
        {
            var result = await _service.GetClientsWithTicketsAsync(pagination);
            return Ok(result);
        }

        //Assign ticket to spec. employee
        [Authorize(Roles = "Manager")]
        [HttpPut("{ticketId}/assign/{employeeId}")]
        public async Task<IActionResult> Assign(Guid ticketId, Guid employeeId)
        {
            await _ticketService.AssignTicketAsync(ticketId, employeeId);
            return Ok(new { message = "Ticket assigned successfully" });
        }

        // List all tickets with status / employees/ external clients 
        [Authorize(Roles = "Manager")]
        [HttpGet("all")]
        public async Task<IActionResult> GetTickets(
            [FromQuery] TicketFilterDto filter,
            [FromQuery] PaginationDto pagination)
        {
            var result = await _ticketService.GetAllTicketsAsync(filter, pagination);
            return Ok(result);
        }

        //view ticket information
        [Authorize(Roles = "Manager")]
        [HttpGet("info/{id}")]
        public async Task<IActionResult> GetTicket(Guid id)
        {
            return Ok(await _ticketService.GetTicketDetailsAsync(id));
        }
    }
}
