//using TicketingSystem.Repository.Data;
//using TicketingSystem.Repository.Models;
//using TicketingSystem.Repository.Repositories.Abstraction;
//using Microsoft.EntityFrameworkCore;

//namespace TicketingSystem.Repository.Repositories
//{
//    public class TicketRepository : GenericRepository<Ticket>, ITicketRepository
//    {
//        public TicketRepository(TicketingSystemDbContext context) : base(context)
//        {
//        }

//        public async Task<List<Ticket>> GetByUserId(Guid userId)
//        {
//            return await _context.Tickets
//                .Where(t => t.UserId == userId)
//                .ToListAsync();
//        }
//    }
//}
