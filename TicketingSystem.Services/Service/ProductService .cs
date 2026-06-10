using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TicketingSystem.Repository.Models;
using TicketingSystem.Repository.UnitOfWork.Abstraction;
using TicketingSystem.Services.DTOs.ProductDtos;
using TicketingSystem.Services.Service.Abstraction;

namespace TicketingSystem.Services.Service
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        //  CREATE
        public async Task<ProductDto> CreateProductAsync(CreateProductDto dto)
        {
            var product = _mapper.Map<Product>(dto);

            product.Id = Guid.NewGuid();
            product.IsActive = true;

            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ProductDto>(product);
        }

        // GET ALL 
        public async Task<List<ProductDto>> GetAllAsync()
        {
            var products = await _unitOfWork.Products
                .Query()
                .Where(p => p.IsActive)
                .ToListAsync();

            return _mapper.Map<List<ProductDto>>(products);
        }

        // GET BY ID 
        public async Task<ProductDto?> GetByIdAsync(Guid id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);

            if (product == null)
                return null;

            return _mapper.Map<ProductDto>(product);
        }

        // TOGGLE STATUS 
        public async Task<ProductDto?> ToggleStatusAsync(Guid id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);

            if (product == null)
                return null;

            product.IsActive = !product.IsActive;

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ProductDto>(product);
        }
    }
}