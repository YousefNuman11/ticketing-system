using TicketingSystem.Services.DTOs.ProductDtos;
using TicketingSystem.Services.Helpers;

namespace TicketingSystem.Services.Service.Abstraction
{
    public interface IProductService
    {
        Task<ProductDto> CreateProductAsync(CreateProductDto dto);
        Task<PagedResult<ProductDto>> GetAllAsync(PaginationDto pagination);
    }
}