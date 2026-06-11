using Microsoft.EntityFrameworkCore;
using TicketingSystem.Repository.Models;
using TicketingSystem.Repository.Specifications.Dashboard;
using TicketingSystem.Repository.Specifications.DashBoard;
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
            var spec = new AllTicketsSpec();

            var query = _unitOfWork.Tickets.Query(spec);

            return await TicketStatusSpec.Apply(query).ToListAsync();
        }

        public async Task<object> GetTopEmployeesAsync()
        {
            var spec = new AllTicketsSpec();

            var query = _unitOfWork.Tickets.Query(spec);

            return await TopEmployeesSpec.Apply(query).ToListAsync();
        }

        public async Task<object> GetTicketTrendAsync()
        {
            var spec = new AllTicketsSpec();

            var query = _unitOfWork.Tickets.Query(spec);

            return await TicketTrendSpec.Apply(query).ToListAsync();
        }
    }
}
