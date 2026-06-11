using TicketingSystem.Repository.Models;

namespace TicketingSystem.Repository.Specifications
{
    public class TicketsByUserSpec : BaseSpecification<Ticket>
    {
        public TicketsByUserSpec(Guid userId)
        {
            Criteria = t => t.UserId == userId;

            AddInclude(t => t.User);
            AddInclude(t => t.AssignedEmployee);
        }
    }
}