using TicketingSystem.Repository.Data;
using TicketingSystem.Repository.Models;
using TicketingSystem.Repository.Repositories.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace TicketingSystem.Repository.Repositories
{
    public class TicketRepository : GenericRepository<Ticket>, ITicketRepository
    {
        public TicketRepository(TicketingSystemDbContext context) : base(context)
        {
        }

        public IQueryable<Ticket> QueryTickets()
        {
            return _context.Tickets
                .Include(t => t.User)
                .Include(t => t.AssignedEmployee);
        }

        public IQueryable<Ticket> GetByUserId(Guid userId)
        {
            return _context.Tickets
                .Where(t => t.UserId == userId);
        }

        public async Task<Ticket?> GetTicketWithDetailsAsync(Guid ticketId)
        {
            return await _context.Tickets
                .Include(t => t.User)
                .Include(t => t.AssignedEmployee)
                .Include(t => t.Product)
                .FirstOrDefaultAsync(t => t.Id == ticketId);
        }

        public async Task<Ticket?> GetTicketDetailsAsync(Guid ticketId)
        {
            return await _context.Tickets
                .Include(t => t.User)
                .Include(t => t.AssignedEmployee)
                .Include(t => t.TicketsComments)
                    .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(t => t.Id == ticketId);
        }
    }
}
