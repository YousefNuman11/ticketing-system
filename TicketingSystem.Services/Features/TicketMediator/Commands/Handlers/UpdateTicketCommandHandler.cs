using AutoMapper;
using MediatR;
using TicketingSystem.Repository.Models;
using TicketingSystem.Repository.UnitOfWork.Abstraction;
using TicketingSystem.Services.DTOs.TicketDtos;

public class UpdateTicketCommandHandler
    : IRequestHandler<UpdateTicketCommand, TicketDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateTicketCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TicketDto?> Handle(
        UpdateTicketCommand request,
        CancellationToken cancellationToken)
    {
        var ticket = await _unitOfWork.Tickets
            .GetByIdAsync(request.TicketId);

        if (ticket == null)
            return null;

        if (ticket.UserId != request.ClientId)
            throw new Exception("Not your ticket");

        if (ticket.Status != TicketStatus.New)
            throw new Exception("Only new tickets can be edited");

        var product = await _unitOfWork.Products
            .GetByIdAsync(request.Dto.ProductId);

        if (product == null)
            throw new Exception("Product not found");

        _mapper.Map(request.Dto, ticket);

        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<TicketDto>(ticket);
    }
}