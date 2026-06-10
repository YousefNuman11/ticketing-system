using TicketingSystem.Repository.Data;
using TicketingSystem.Repository.Models;
using TicketingSystem.Repository.Repositories.Abstraction;

namespace TicketingSystem.Repository.Repositories
{
    public class TicketAttachmentRepository : GenericRepository<TicketAttachment>, ITicketAttachmentRepository
    {
        private readonly TicketingSystemDbContext _context;

        public TicketAttachmentRepository(TicketingSystemDbContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<TicketAttachment> GetByTicketId(Guid ticketId)
        {
            return _context.TicketAttachments
                .Where(a => a.TicketId == ticketId);
        }
    }
}