using MediatR;
using TicketingSystem.Repository.Models;
using TicketingSystem.Repository.Specifications.Tickets;
using TicketingSystem.Repository.UnitOfWork.Abstraction;
using TicketingSystem.Services.AI;
using TicketingSystem.Services.AI.Abstraction;
using TicketingSystem.Services.Exceptions;

namespace TicketingSystem.Services.Features.TicketMediator.Commands
{
    public class ResolveTicketCommandHandler
        : IRequestHandler<ResolveTicketCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmbeddingService _embeddingService;

        public ResolveTicketCommandHandler(
            IUnitOfWork unitOfWork, IEmbeddingService embeddingService)
        {
            _unitOfWork = unitOfWork;
            _embeddingService = embeddingService;
        }

        public async Task Handle(
            ResolveTicketCommand request,
            CancellationToken cancellationToken)
        {

            var spec = new TicketWithCommentsByIdSpec(request.TicketId);
            var ticket = (await _unitOfWork.Tickets.ListAsync(spec)).FirstOrDefault();

            if (ticket == null)
                throw new NotFoundException("Ticket not found");

            if (ticket.AssignedEmployeeId != request.EmployeeId)
                throw new ForbiddenException("Not assigned to you");

            ticket.Status = TicketStatus.Resolved;

            await _unitOfWork.SaveChangesAsync();

            try
            {
                var textToEmbed = TicketEmbeddingTextBuilder.Build(ticket);
                var embedding = await _embeddingService.EmbedAsync(textToEmbed, cancellationToken);

                ticket.EmbeddingJson = System.Text.Json.JsonSerializer.Serialize(embedding);
                await _unitOfWork.SaveChangesAsync();
            }
            catch
            {

            }
        }
    }
}