using TicketingSystem.Repository.Models;

namespace TicketingSystem.Repository.Specifications
{
    public class AssignedTicketsSpec : BaseSpecification<Ticket>
    {
        public AssignedTicketsSpec(Guid employeeId)
        {
            Criteria = t => t.AssignedEmployeeId == employeeId;

            AddInclude(t => t.User);
            AddInclude(t => t.AssignedEmployee);
        }
    }
}