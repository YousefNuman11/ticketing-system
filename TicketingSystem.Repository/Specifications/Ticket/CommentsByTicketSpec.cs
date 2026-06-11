namespace TicketingSystem.Repository.Specifications
{
    public class CommentsByTicketSpec : BaseSpecification<TicketsComment>
    {
        public CommentsByTicketSpec(Guid ticketId)
        {
            Criteria = c => c.TicketId == ticketId;

            AddInclude(c => c.User);
        }
    }
}