using TicketingSystem.Repository.Models;

namespace TicketingSystem.Repository.Specifications
{
    public class TicketsByUserSpec : BaseSpecification<Ticket>
    {
        public TicketsByUserSpec(Guid userId, string? search = null)
        {
            Criteria = t => t.UserId == userId &&
                (string.IsNullOrWhiteSpace(search) ||
                 t.Title.Contains(search) ||
                 (t.Description != null && t.Description.Contains(search)));
        }
    }
}