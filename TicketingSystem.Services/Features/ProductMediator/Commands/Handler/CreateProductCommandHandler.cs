using MediatR;
using TicketingSystem.Services.DTOs.ProductDtos;
using TicketingSystem.Services.Service.Abstraction;

namespace TicketingSystem.API.Features.ProductMediator.Commands.CreateProduct
{
    public class CreateProductCommandHandler
        : IRequestHandler<CreateProductCommand, ProductDto>
    {
        private readonly IProductService _productService;

        public CreateProductCommandHandler(
            IProductService productService)
        {
            _productService = productService;
        }

        public async Task<ProductDto> Handle(
            CreateProductCommand request,
            CancellationToken cancellationToken)
        {
            return await _productService
                .CreateProductAsync(request.ProductDto);
        }
    }
}