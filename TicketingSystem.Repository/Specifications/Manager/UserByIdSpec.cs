using TicketingSystem.Repository.Models;

namespace TicketingSystem.Repository.Specifications.Users
{
    public class UserByIdSpec : BaseSpecification<User>
    {
        public UserByIdSpec(Guid id)
        {
            Criteria = u => u.Id == id;
        }
    }
}
