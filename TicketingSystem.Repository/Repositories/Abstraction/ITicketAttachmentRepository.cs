using TicketingSystem.Repository.Models;

namespace TicketingSystem.Repository.Repositories.Abstraction
{
    public interface ITicketAttachmentRepository : IGenericRepository<TicketAttachment>
    {
        IQueryable<TicketAttachment> GetByTicketId(Guid ticketId);
    }
}