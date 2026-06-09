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

        public IGenericRepository<User> Users { get; }
        public IGenericRepository<Product> Products { get; }
        public IGenericRepository<Ticket> Tickets { get; }
        public IGenericRepository<TicketsComment> TicketsComments { get; }
        public IGenericRepository<TicketAttachment> TicketAttachments { get; }

        public UnitOfWork(TicketingSystemDbContext context)
        {
            _context = context;

            Users = new GenericRepository<User>(context);
            Products = new GenericRepository<Product>(context);
            Tickets = new GenericRepository<Ticket>(context);
            TicketsComments = new GenericRepository<TicketsComment>(context);
            TicketAttachments = new GenericRepository<TicketAttachment>(context);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
