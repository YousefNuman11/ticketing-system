using MediatR;
using TicketingSystem.Services.DTOs.ProductDtos;
using TicketingSystem.Services.Helpers;

namespace TicketingSystem.API.Features.ProductMediator.Queries.GetAllProducts
{
    public record GetAllProductsQuery(
        PaginationDto Pagination
    ) : IRequest<PagedResult<ProductDto>>;
}