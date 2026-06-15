using TicketingSystem.Repository.Models;

namespace TicketingSystem.Repository.Repositories.Abstraction
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        IQueryable<Product> GetProductsQuery();
    }
}