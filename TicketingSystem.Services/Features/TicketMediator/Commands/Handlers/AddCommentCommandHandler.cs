using AutoMapper;
using MediatR;
using TicketingSystem.Repository.Models;
using TicketingSystem.Repository.UnitOfWork.Abstraction;
using TicketingSystem.Services.DTOs.CommentDtos;

public class AddCommentCommandHandler
    : IRequestHandler<AddCommentCommand, CommentDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddCommentCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CommentDto> Handle(
        AddCommentCommand request,
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

        var comment = _mapper.Map<TicketsComment>(
            request.Dto);

        comment.Id = Guid.NewGuid();
        comment.TicketId = request.TicketId;
        comment.UserId = request.UserId;
        comment.CreatedAt = DateTime.UtcNow;

        await _unitOfWork.TicketsComments
            .AddAsync(comment);

        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<CommentDto>(comment);
    }
}