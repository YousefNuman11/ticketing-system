using System;
using System.Collections.Generic;
using System.Text;

namespace TicketingSystem.Services.DTOs.TicketAttachmentDto
{
    public class AttachmentDto
    {
        public Guid Id { get; set; }

        public string FileName { get; set; } = string.Empty;

        public string FilePath { get; set; } = string.Empty;
    }
}
