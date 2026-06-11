using Microsoft.AspNetCore.Http;

namespace TicketingSystem.Services.DTOs.TicketAttachmentDto
{
    public class UploadAttachmentDto
    {
        public string FileName { get; set; } = string.Empty;
    }
}