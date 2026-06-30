using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Services.Features.ProductMediator.Commands;
using TicketingSystem.Services.Features.ProductMediator.Contracts;
using TicketingSystem.Services.Features.ProductMediator.Queries;
using TicketingSystem.Services.Helpers;

namespace TicketingSystem.API.Controllers
{
    [ApiController]
    [Route("api/products")]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Only managers can create products
        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
            => Ok(await _mediator.Send(new CreateProductCommand(dto)));

        // Any authenticated user can list products (clients need them to open a ticket)
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationDto pagination,
            [FromQuery] string? search)
            => Ok(await _mediator.Send(new GetAllProductsQuery(pagination, search)));
    }
}
