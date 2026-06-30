using TicketingSystem.Repository.Models;

namespace TicketingSystem.Repository.Specifications.Users
{
    public class UsersByIdsSpec : BaseSpecification<User>
    {
        public UsersByIdsSpec(List<Guid> ids, UserRole role)
        {
            Criteria = u => ids.Contains(u.Id) && u.Role == role;
        }
    }
}