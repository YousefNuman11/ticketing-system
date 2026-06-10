using Microsoft.EntityFrameworkCore;
using TicketingSystem.Repository.Models;
using TicketingSystem.Repository.UnitOfWork.Abstraction;
using TicketingSystem.Services.Service.Abstraction;

namespace TicketingSystem.Services.Service
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<object> GetTicketStatusAsync()
        {
            return await _unitOfWork.Tickets.Query()
                .GroupBy(t => t.Status)
                .Select(g => new
                {
                    Status = g.Key.ToString(),
                    Count = g.Count()
                })
                .ToListAsync();
        }

        public async Task<object> GetTopEmployeesAsync()
        {
            var result = await _unitOfWork.Tickets.Query()
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
                .Take(5)
                .ToListAsync();

            return result;
        }

        public async Task<object> GetTicketTrendAsync()
        {
            return await _unitOfWork.Tickets.Query()
                .GroupBy(t => t.CreatedAt.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Count = g.Count()
                })
                .OrderBy(x => x.Date)
                .ToListAsync();
        }
    }
}
