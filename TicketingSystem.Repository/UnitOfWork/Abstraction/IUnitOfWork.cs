using TicketingSystem.Repository.Repositories.Abstraction;
using TicketingSystem.Repository.Models;

namespace TicketingSystem.Repository.UnitOfWork.Abstraction
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        ITicketRepository Tickets { get; }
        IProductRepository Products { get; }
        ITicketCommentRepository TicketsComments { get; }
        ITicketAttachmentRepository TicketAttachments { get; }

        Task<int> SaveChangesAsync();
    }
}