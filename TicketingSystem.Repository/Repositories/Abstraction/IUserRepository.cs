using TicketingSystem.Repository.Models;

namespace TicketingSystem.Repository.Repositories.Abstraction
{
    public interface IUserRepository : IGenericRepository<User>
    {
        IQueryable<User> GetUsersQuery();


        IQueryable<User> GetByRole(UserRole role);

        IQueryable<User> GetClientsWithTickets();
    }
}