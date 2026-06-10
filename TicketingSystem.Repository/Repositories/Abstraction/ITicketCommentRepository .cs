using TicketingSystem.Repository.Models;

namespace TicketingSystem.Repository.Repositories.Abstraction
{
    public interface ITicketCommentRepository : IGenericRepository<TicketsComment>
    {
        IQueryable<TicketsComment> GetByTicketId(Guid ticketId);
    }
}