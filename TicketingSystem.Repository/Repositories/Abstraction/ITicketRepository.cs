using TicketingSystem.Repository.Models;

namespace TicketingSystem.Repository.Repositories.Abstraction
{
    public interface ITicketRepository : IGenericRepository<Ticket>
    {
        IQueryable<Ticket> QueryTickets();

        Task<Ticket?> GetTicketWithDetailsAsync(Guid ticketId);

        IQueryable<Ticket> GetByUserId(Guid userId);

        Task<Ticket?> GetTicketDetailsAsync(Guid ticketId);
    }
}