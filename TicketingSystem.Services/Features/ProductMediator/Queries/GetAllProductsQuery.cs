using MediatR;
using TicketingSystem.Services.Features.ProductMediator.Contracts;
using TicketingSystem.Services.Helpers;

namespace TicketingSystem.Services.Features.ProductMediator.Queries
{
    public record GetAllProductsQuery(PaginationDto Pagination)
        : IRequest<PagedResult<ProductDto>>;
}
