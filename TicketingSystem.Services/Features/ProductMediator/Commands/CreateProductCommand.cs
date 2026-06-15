using MediatR;
using TicketingSystem.Services.Features.ProductMediator.Contracts;

namespace TicketingSystem.Services.Features.ProductMediator.Commands
{
    public record CreateProductCommand(CreateProductDto ProductDto) : IRequest<ProductDto>;
}
