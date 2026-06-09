using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Services.DTOs;
using TicketingSystem.Services.Service.Abstraction;

namespace TicketingSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Manager")]
    public class ManagerController : ControllerBase
    {
        private readonly IManagerService _service;

        public ManagerController(IManagerService service)
        {
            _service = service;
        }

        [HttpPost("employee")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> CreateEmployee(CreateEmployeeDto dto)
        {
            var result = await _service.CreateEmployeeAsync(dto);
            return Ok(result);
        }

        [Authorize(Roles = "Manager")]
        [HttpGet("employees")]
        public async Task<IActionResult> GetEmployees()
            => Ok(await _service.GetEmployeesAsync());

        [Authorize(Roles = "Manager")]
        [HttpGet("clients")]
        public async Task<IActionResult> GetClients()
            => Ok(await _service.GetClientsAsync());

        [Authorize(Roles = "Manager")]
        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var user = await _service.GetUserByIdAsync(id);
            return user == null ? NotFound() : Ok(user);
        }

        [Authorize(Roles = "Manager")]
        [HttpPut("users/{id}/toggle-status")]
        public async Task<IActionResult> Toggle(Guid id)
        {
            var user = await _service.ToggleUserStatusAsync(id);
            return user == null ? NotFound() : Ok(user);
        }

        [Authorize(Roles = "Manager")]
        [HttpPut("users/{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateUserDto dto)
        {
            var user = await _service.UpdateUserAsync(id, dto);
            return user == null ? NotFound() : Ok(user);
        }
    }
}
