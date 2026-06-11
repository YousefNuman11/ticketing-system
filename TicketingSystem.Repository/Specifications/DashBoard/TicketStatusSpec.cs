using TicketingSystem.Repository.Models;

namespace TicketingSystem.Repository.Specifications.Dashboard
{
    public class TicketStatusSpec
    {
        public static IQueryable<object> Apply(IQueryable<Ticket> query)
        {
            return query
                .GroupBy(t => t.Status)
                .Select(g => new
                {
                    Status = g.Key.ToString(),
                    Count = g.Count()
                });
        }
    }
}