using TicketingSystem.Repository.Models;

namespace TicketingSystem.Repository.Repositories.Abstraction
{
    public interface ITicketRepository : IGenericRepository<Ticket>
    {
        Task<List<Ticket>> GetTicketsInformation();
        Task<List<Ticket>> GetByUserId(Guid userId);
    }
}