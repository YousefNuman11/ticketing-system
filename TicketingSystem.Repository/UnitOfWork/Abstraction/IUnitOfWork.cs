using TicketingSystem.Repository.Models;
using TicketingSystem.Repository.Repositories.Abstraction;

namespace TicketingSystem.Repository.UnitOfWork.Abstraction
{
    public interface IUnitOfWork
    {
        IGenericRepository<User> Users { get; }
        IGenericRepository<Product> Products { get; }
        IGenericRepository<Ticket> Tickets { get; }
        IGenericRepository<TicketsComment> TicketsComments { get; }
        IGenericRepository<TicketAttachment> TicketAttachments { get; }
        Task<int> SaveChangesAsync();
    }
}
