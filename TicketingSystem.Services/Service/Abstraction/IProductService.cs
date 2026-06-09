using TicketingSystem.Services.DTOs.ProductDtos;

namespace TicketingSystem.Services.Service.Abstraction
{
    public interface IProductService
    {
        Task<ProductDto> CreateProductAsync(CreateProductDto dto);
        Task<List<ProductDto>> GetAllAsync();
    }
}