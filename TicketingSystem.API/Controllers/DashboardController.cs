using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Services.Service.Abstraction;

namespace TicketingSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Manager")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _service;

        public DashboardController(IDashboardService service)
        {
            _service = service;
        }

        [HttpGet("status")]
        public async Task<IActionResult> Status()
            => Ok(await _service.GetTicketStatusAsync());

        [HttpGet("top-employees")]
        public async Task<IActionResult> TopEmployees()
            => Ok(await _service.GetTopEmployeesAsync());

        [HttpGet("trend")]
        public async Task<IActionResult> Trend()
            => Ok(await _service.GetTicketTrendAsync());
    }
}
