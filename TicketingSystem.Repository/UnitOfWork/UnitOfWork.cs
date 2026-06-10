using TicketingSystem.Repository.Data;
using TicketingSystem.Repository.Models;
using TicketingSystem.Repository.Repositories;
using TicketingSystem.Repository.Repositories.Abstraction;
using TicketingSystem.Repository.UnitOfWork.Abstraction;

namespace TicketingSystem.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TicketingSystemDbContext _context;

        public IUserRepository Users { get; }
        public ITicketRepository Tickets { get; }
        public IProductRepository Products { get; }
        public ITicketCommentRepository TicketsComments { get; }
        public ITicketAttachmentRepository TicketAttachments { get; }

        public UnitOfWork(TicketingSystemDbContext context)
        {
            _context = context;

            Users = new UserRepository(context);
            Tickets = new TicketRepository(context);
            Products = new ProductRepository(context);
            TicketsComments = new TicketCommentRepository(context);
            TicketAttachments = new TicketAttachmentRepository(context);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}