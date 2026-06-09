using TicketingSystem.Repository.Models;
using TicketingSystem.Repository.Repositories.Abstraction;

namespace TicketingSystem.Repository.UnitOfWork.Abstraction
{
    public interface IUnitOfWork
    {
        IGenericRepository<User> Users { get; }
        Task<int> SaveChangesAsync();
    }
}
