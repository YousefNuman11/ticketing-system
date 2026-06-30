using TicketingSystem.Repository.Models;

namespace TicketingSystem.Repository.Specifications.Tickets
{
    public class TicketWithCommentsByIdSpec : BaseSpecification<Ticket>
    {
        public TicketWithCommentsByIdSpec(Guid ticketId)
        {
            Criteria = t => t.Id == ticketId;
            AddInclude(t => t.TicketsComments);
        }
    }
}