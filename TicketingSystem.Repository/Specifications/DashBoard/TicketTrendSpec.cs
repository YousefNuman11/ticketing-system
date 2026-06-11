using TicketingSystem.Repository.Models;

namespace TicketingSystem.Repository.Specifications.Dashboard
{
    public class TicketTrendSpec
    {
        public static IQueryable<object> Apply(IQueryable<Ticket> query)
        {
            return query
                .GroupBy(t => t.CreatedAt.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Count = g.Count()
                })
                .OrderBy(x => x.Date);
        }
    }
}