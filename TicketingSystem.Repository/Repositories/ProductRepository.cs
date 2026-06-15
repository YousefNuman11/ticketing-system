using Microsoft.EntityFrameworkCore;
using TicketingSystem.Repository.Data;
using TicketingSystem.Repository.Models;
using TicketingSystem.Repository.Repositories.Abstraction;

namespace TicketingSystem.Repository.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly TicketingSystemDbContext _context;

        public ProductRepository(TicketingSystemDbContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<Product> GetProductsQuery()
        {
            return _context.Products.AsQueryable();
        }
    }
}