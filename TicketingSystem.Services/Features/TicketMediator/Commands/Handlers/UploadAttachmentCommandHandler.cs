using AutoMapper;
using MediatR;
using TicketingSystem.Repository.Models;
using TicketingSystem.Repository.UnitOfWork.Abstraction;
using TicketingSystem.Services.DTOs.TicketAttachmentDto;

public class UploadAttachmentCommandHandler
    : IRequestHandler<UploadAttachmentCommand, AttachmentDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UploadAttachmentCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AttachmentDto> Handle(
        UploadAttachmentCommand request,
        CancellationToken cancellationToken)
    {
        var ticket = await _unitOfWork.Tickets
            .GetByIdAsync(request.TicketId);

        if (ticket == null)
            throw new Exception("Ticket not found");

        if (ticket.UserId != request.UserId &&
            ticket.AssignedEmployeeId != request.UserId)
        {
            throw new Exception("Not allowed");
        }

        var uploadsFolder = Path.Combine(
            Directory.GetCurrentDirectory(),
            "Uploads");

        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var uniqueFileName =
            $"{Guid.NewGuid()}_{request.File.FileName}";

        var filePath = Path.Combine(
            uploadsFolder,
            uniqueFileName);

        using (var stream =
               new FileStream(filePath, FileMode.Create))
        {
            await request.File.CopyToAsync(stream);
        }

        var attachment = new TicketAttachment
        {
            Id = Guid.NewGuid(),
            TicketId = request.TicketId,
            FileName = request.File.FileName,
            FileUrl = uniqueFileName,
            UploadedBy = request.UserId,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.TicketAttachments
            .AddAsync(attachment);

        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<AttachmentDto>(attachment);
    }
}