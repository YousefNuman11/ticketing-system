using TicketingSystem.Repository.Models;

namespace TicketingSystem.Repository.Specifications.Users
{
    public class UsersByIdsWithTicketsSpec : BaseSpecification<User>
    {
        public UsersByIdsWithTicketsSpec(List<Guid> ids, UserRole role)
        {
            Criteria = u => ids.Contains(u.Id) && u.Role == role;
            AddInclude(u => u.CreatedTickets);
        }
    }
}