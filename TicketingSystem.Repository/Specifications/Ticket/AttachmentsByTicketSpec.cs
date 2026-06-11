using TicketingSystem.Repository.Models;

namespace TicketingSystem.Repository.Specifications
{
    public class AttachmentsByTicketSpec : BaseSpecification<TicketAttachment>
    {
        public AttachmentsByTicketSpec(Guid ticketId)
        {
            Criteria = a => a.TicketId == ticketId;
        }
    }
}