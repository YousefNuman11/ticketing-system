using AutoMapper;
using TicketingSystem.Repository.Models;
using TicketingSystem.Repository.UnitOfWork.Abstraction;
using TicketingSystem.Services.DTOs.CommentDtos;
using TicketingSystem.Services.DTOs.TicketDtos;
using TicketingSystem.Services.Service.Abstraction;

namespace TicketingSystem.Services.Service
{
    public class TicketService : ITicketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TicketService(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TicketDto> CreateTicketAsync(
            CreateTicketDto dto,
            Guid clientId)
        {
            var product =
                await _unitOfWork.Products.GetByIdAsync(dto.ProductId);

            if (product == null)
                throw new Exception("Product not found");

            var ticket = _mapper.Map<Ticket>(dto);

            ticket.Id = Guid.NewGuid();
            ticket.UserId = clientId;
            ticket.Status = TicketStatus.New;

            await _unitOfWork.Tickets.AddAsync(ticket);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<TicketDto>(ticket);
        }

        public async Task<List<TicketDto>> GetMyTicketsAsync(Guid clientId)
        {
            var tickets = await _unitOfWork.Tickets.GetAllAsync();

            var result = tickets
                .Where(t => t.UserId == clientId)
                .ToList();

            return _mapper.Map<List<TicketDto>>(result);
        }
        public async Task<TicketDto?> GetTicketByIdAsync(Guid ticketId, Guid clientId)
        {
            var tickets = await _unitOfWork.Tickets.GetAllAsync();

            var ticket = tickets
                .FirstOrDefault(t => t.Id == ticketId && t.UserId == clientId);

            if (ticket == null)
                return null;

            return _mapper.Map<TicketDto>(ticket);
        }

        public async Task AssignTicketAsync(Guid ticketId, Guid employeeId)
        {
            var ticket = await _unitOfWork.Tickets.GetByIdAsync(ticketId);

            if (ticket == null)
                throw new Exception("Ticket not found");

            if (ticket.AssignedEmployeeId != null)
                throw new Exception("Ticket already assigned");

            var employee = await _unitOfWork.Users.GetByIdAsync(employeeId);

            if (employee == null || employee.Role != UserRole.Employee)
                throw new Exception("Invalid employee");

            ticket.AssignedEmployeeId = employeeId;
            ticket.Status = TicketStatus.InProgress;

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<TicketDto>> GetMyAssignedTicketsAsync(Guid employeeId)
        {
            var tickets = await _unitOfWork.Tickets.GetAllAsync();

            var result = tickets
                .Where(t => t.AssignedEmployeeId == employeeId)
                .ToList();

            return _mapper.Map<List<TicketDto>>(result);
        }

        public async Task<CommentDto> AddCommentAsync(Guid ticketId,Guid userId,AddCommentDto dto)
        {
            var ticket = await _unitOfWork.Tickets.GetByIdAsync(ticketId);

            if (ticket == null)
                throw new Exception("Ticket not found");

            var comment = _mapper.Map<TicketsComment>(dto);

            comment.Id = Guid.NewGuid();
            comment.TicketId = ticketId;
            comment.UserId = userId;
            comment.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.TicketsComments.AddAsync(comment);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CommentDto>(comment);
        }

        public async Task<List<CommentDto>> GetCommentsAsync(Guid ticketId)
        {
            var comments =
                await _unitOfWork.TicketsComments.GetAllAsync();

            return comments
                .Where(c => c.TicketId == ticketId)
                .Select(c => new CommentDto
                {
                    Id = c.Id,
                    Text = c.Text,
                    CreatedAt = c.CreatedAt
                })
                .ToList();
        }

        public async Task ResolveTicketAsync(Guid ticketId,Guid employeeId)
        {
            var ticket = await _unitOfWork.Tickets.GetByIdAsync(ticketId);

            if (ticket == null)
                throw new Exception("Ticket not found");

            if (ticket.AssignedEmployeeId != employeeId)
                throw new Exception("You are not assigned to this ticket");

            ticket.Status = TicketStatus.Resolved;

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task CloseTicketAsync(Guid ticketId,Guid clientId)
        {
            var ticket = await _unitOfWork.Tickets.GetByIdAsync(ticketId);

            if (ticket == null)
                throw new Exception("Ticket not found");

            if (ticket.UserId != clientId)
                throw new Exception("This ticket does not belong to you");

            if (ticket.Status != TicketStatus.Resolved)
                throw new Exception(
                    "Ticket must be resolved before closing");

            ticket.Status = TicketStatus.Closed;

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
