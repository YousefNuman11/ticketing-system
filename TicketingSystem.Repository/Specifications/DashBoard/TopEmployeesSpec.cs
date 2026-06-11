using Microsoft.EntityFrameworkCore;
using TicketingSystem.Repository.Models;

namespace TicketingSystem.Repository.Specifications.Dashboard
{
    public class TopEmployeesSpec
    {
        public static IQueryable<object> Apply(IQueryable<Ticket> query)
        {
            return query
                .Where(t => t.AssignedEmployeeId != null)
                .Include(t => t.AssignedEmployee)
                .GroupBy(t => new
                {
                    t.AssignedEmployeeId,
                    t.AssignedEmployee.FullName
                })
                .Select(g => new
                {
                    EmployeeName = g.Key.FullName,
                    ResolvedCount = g.Count(x => x.Status == TicketStatus.Resolved)
                })
                .OrderByDescending(x => x.ResolvedCount)
                .Take(5);
        }
    }
}