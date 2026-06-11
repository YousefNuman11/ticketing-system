using TicketingSystem.Repository.Models;
using TicketingSystem.Repository.Specifications;

public class UserByIdSpec : BaseSpecification<User>
{
    public UserByIdSpec(Guid id)
    {
        Criteria = u => u.Id == id;
    }
}