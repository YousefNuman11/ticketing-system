using TicketingSystem.Repository.Models;
using TicketingSystem.Repository.Repositories.Abstraction;

public interface IUserRepository
    : IGenericRepository<User>
{
    Task<IEnumerable<User>> GetEmployeesAsync();

    Task<IEnumerable<User>> GetClientsAsync();
}