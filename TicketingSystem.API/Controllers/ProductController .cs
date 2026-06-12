using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketingSystem.API.Features.ProductMediator.Commands.CreateProduct;
using TicketingSystem.API.Features.ProductMediator.Queries.GetAllProducts;
using TicketingSystem.Services.DTOs.ProductDtos;
using TicketingSystem.Services.Helpers;

namespace TicketingSystem.API.Controllers
{
    [ApiController]
    [Route("api/products")]
    [Authorize(Roles = "Manager")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
        {
            var result = await _mediator.Send(
                new CreateProductCommand(dto));
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(PaginationDto pagination)
        {
            return Ok(await _mediator.Send(
                new GetAllProductsQuery(pagination)));
        }
    }
}
