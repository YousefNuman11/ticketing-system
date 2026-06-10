using Microsoft.EntityFrameworkCore;
using TicketingSystem.Repository.Data;
using TicketingSystem.Repository.Models;
using TicketingSystem.Repository.Repositories.Abstraction;

namespace TicketingSystem.Repository.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly TicketingSystemDbContext _context;

        public UserRepository(TicketingSystemDbContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<User> GetUsersQuery()
        {
            return _context.Users.AsQueryable();
        }

        public IQueryable<User> GetByRole(UserRole role)
        {
            return _context.Users.Where(u => u.Role == role);
        }

        public IQueryable<User> GetClientsWithTickets()
        {
            return _context.Users
                .Where(u => u.Role == UserRole.Client)
                .Include(u => u.CreatedTickets);
        }
    }
}