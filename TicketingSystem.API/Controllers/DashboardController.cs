using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketingSystem.API.Features.Dashboard.Queries.GetTicketStatus;
using TicketingSystem.API.Features.Dashboard.Queries.GetTicketTrend;
using TicketingSystem.API.Features.Dashboard.Queries.GetTopEmployees;
using TicketingSystem.Services.Service.Abstraction;

namespace TicketingSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Manager")]
    public class DashboardController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DashboardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("status")]
        public async Task<IActionResult> Status()
            => Ok(await _mediator.Send(
                new GetTicketStatusQuery()));

        [HttpGet("top-employees")]
        public async Task<IActionResult> TopEmployees()
            => Ok(await _mediator.Send(
                new GetTopEmployeesQuery()));

        [HttpGet("trend")]
        public async Task<IActionResult> Trend()
            => Ok(await _mediator.Send(
                new GetTicketTrendQuery()));
    }
}