using TicketingSystem.Repository.Data;
using TicketingSystem.Repository.Models;
using TicketingSystem.Repository.Repositories;
using TicketingSystem.Repository.Repositories.Abstraction;
using TicketingSystem.Repository.UnitOfWork.Abstraction;

namespace TicketingSystem.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TicketingSystemDbContext _context;

        public IGenericRepository<User> Users { get; }

        public UnitOfWork(TicketingSystemDbContext context)
        {
            _context = context;
            Users = new GenericRepository<User>(context);
        }

        public Task<int> SaveChangesAsync()
            => _context.SaveChangesAsync();
    }
}
