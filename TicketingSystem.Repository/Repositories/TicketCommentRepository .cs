using TicketingSystem.Repository.Data;
using TicketingSystem.Repository.Repositories.Abstraction;

namespace TicketingSystem.Repository.Repositories
{
    public class TicketCommentRepository : GenericRepository<TicketsComment>, ITicketCommentRepository
    {
        private readonly TicketingSystemDbContext _context;

        public TicketCommentRepository(TicketingSystemDbContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<TicketsComment> GetByTicketId(Guid ticketId)
        {
            return _context.TicketsComments
                .Where(c => c.TicketId == ticketId);
        }
    }
}