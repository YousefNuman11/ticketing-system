using MediatR;
using TicketingSystem.Services.DTOs.ProductDtos;
using TicketingSystem.Services.Helpers;
using TicketingSystem.Services.Service.Abstraction;

namespace TicketingSystem.API.Features.ProductMediator.Queries.GetAllProducts
{
    public class GetAllProductsQueryHandler
        : IRequestHandler<
            GetAllProductsQuery,
            PagedResult<ProductDto>>
    {
        private readonly IProductService _productService;

        public GetAllProductsQueryHandler(
            IProductService productService)
        {
            _productService = productService;
        }

        public async Task<PagedResult<ProductDto>> Handle(
            GetAllProductsQuery request,
            CancellationToken cancellationToken)
        {
            return await _productService
                .GetAllAsync(request.Pagination);
        }
    }
}