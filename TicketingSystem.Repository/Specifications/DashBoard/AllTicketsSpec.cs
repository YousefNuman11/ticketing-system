using TicketingSystem.Repository.Models;

namespace TicketingSystem.Repository.Specifications.DashBoard
{
    public class AllTicketsSpec : BaseSpecification<Ticket>
    {
        public AllTicketsSpec()
        {
            // no filter = all tickets
        }
    }
}
