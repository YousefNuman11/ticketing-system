namespace TicketingSystem.Services.Service.Abstraction
{
    public interface IDashboardService
    {
        Task<object> GetTicketStatusAsync();
        Task<object> GetTopEmployeesAsync();
        Task<object> GetTicketTrendAsync();
    }
}
