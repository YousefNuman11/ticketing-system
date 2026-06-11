using TicketingSystem.Repository.Models;

namespace TicketingSystem.Repository.Specifications.Users
{
    public class EmployeesSpec : BaseSpecification<User>
    {
        public EmployeesSpec()
        {
            Criteria = u => u.Role == UserRole.Employee;
        }
    }
}