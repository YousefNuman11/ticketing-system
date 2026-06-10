using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Services.DTOs.User;
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

        //Register Employee
        [HttpPost("employee")]
        public async Task<IActionResult> CreateEmployee(CreateEmployeeDto dto)
        {
            var result = await _service.CreateEmployeeAsync(dto);
            return Ok(result);
        }

        //Get a List of Employees
        [HttpGet("employees")]
        public async Task<IActionResult> GetEmployees()
            => Ok(await _service.GetEmployeesAsync());

        [HttpGet("clients")]
        public async Task<IActionResult> GetClients()
            => Ok(await _service.GetClientsAsync());

        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var user = await _service.GetUserByIdAsync(id);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpPut("users/{id}/toggle-status")]
        public async Task<IActionResult> Toggle(Guid id)
        {
            var user = await _service.ToggleUserStatusAsync(id);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpPut("users/{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateUserDto dto)
        {
            var user = await _service.UpdateUserAsync(id, dto);
            return user == null ? NotFound() : Ok(user);
        }

    }
}
