using TicketingSystem.Repository.Models;

namespace TicketingSystem.Repository.Specifications
{
    public class TicketDetailsSpec : BaseSpecification<Ticket>
    {
        public TicketDetailsSpec(Guid ticketId)
        {
            Criteria = t => t.Id == ticketId;

            AddInclude(t => t.User);
            AddInclude(t => t.AssignedEmployee);
            AddInclude(t => t.Product);
            AddInclude(t => t.TicketsComments);
        }
    }
}