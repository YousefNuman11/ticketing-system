using MediatR;
using TicketingSystem.Services.DTOs.ProductDtos;

namespace TicketingSystem.API.Features.ProductMediator.Commands.CreateProduct
{
    public record CreateProductCommand(
        CreateProductDto ProductDto
    ) : IRequest<ProductDto>;
}