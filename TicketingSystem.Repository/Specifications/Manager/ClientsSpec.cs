using TicketingSystem.Repository.Models;

namespace TicketingSystem.Repository.Specifications.Users
{
    public class ClientsSpec : BaseSpecification<User>
    {
        public ClientsSpec()
        {
            Criteria = u => u.Role == UserRole.Client;
            AddInclude(u => u.CreatedTickets);
        }
    }
}