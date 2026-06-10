using AutoMapper;
using Microsoft.EntityFrameworkCore;
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

        public TicketService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        //CREATE 
        public async Task<TicketDto> CreateTicketAsync(CreateTicketDto dto, Guid clientId)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(dto.ProductId);

            if (product == null)
                throw new Exception("Product not found");

            var ticket = _mapper.Map<Ticket>(dto);

            ticket.Id = Guid.NewGuid();
            ticket.UserId = clientId;
            ticket.Status = TicketStatus.New;
            ticket.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.Tickets.AddAsync(ticket);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<TicketDto>(ticket);
        }

        //UPDATE 
        public async Task<TicketDto?> UpdateTicketAsync(Guid ticketId, Guid clientId, UpdateTicketDto dto)
        {
            var ticket = await _unitOfWork.Tickets.GetByIdAsync(ticketId);

            if (ticket == null)
                return null;

            if (ticket.UserId != clientId)
                throw new Exception("Not your ticket");

            if (ticket.Status != TicketStatus.New)
                throw new Exception("Only new tickets can be edited");

            var product = await _unitOfWork.Products.GetByIdAsync(dto.ProductId);

            if (product == null)
                throw new Exception("Product not found");

            _mapper.Map(dto, ticket);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<TicketDto>(ticket);
        }

        // MY TICKETS 
        public async Task<List<TicketDto>> GetMyTicketsAsync(Guid clientId)
        {
            var tickets = await _unitOfWork.Tickets
                .GetByUserId(clientId)
                .ToListAsync();

            return _mapper.Map<List<TicketDto>>(tickets);
        }

        // BY ID 
        public async Task<TicketDto?> GetTicketByIdAsync(Guid ticketId, Guid clientId)
        {
            var ticket = await _unitOfWork.Tickets
                .GetByUserId(clientId)
                .FirstOrDefaultAsync(t => t.Id == ticketId);

            return ticket == null ? null : _mapper.Map<TicketDto>(ticket);
        }

        // ASSIGN
        public async Task AssignTicketAsync(Guid ticketId, Guid employeeId)
        {
            var ticket = await _unitOfWork.Tickets.GetByIdAsync(ticketId);

            if (ticket == null)
                throw new Exception("Ticket not found");

            if (ticket.AssignedEmployeeId != null)
                throw new Exception("Already assigned");

            var employee = await _unitOfWork.Users.GetByIdAsync(employeeId);

            if (employee == null || employee.Role != UserRole.Employee)
                throw new Exception("Invalid employee");

            ticket.AssignedEmployeeId = employeeId;
            ticket.Status = TicketStatus.InProgress;

            await _unitOfWork.SaveChangesAsync();
        }

        // MY ASSIGNED
        public async Task<List<TicketDto>> GetMyAssignedTicketsAsync(Guid employeeId)
        {
            var tickets = await _unitOfWork.Tickets
                .QueryTickets()
                .Where(t => t.AssignedEmployeeId == employeeId)
                .ToListAsync();

            return _mapper.Map<List<TicketDto>>(tickets);
        }

        // COMMENTS
        public async Task<CommentDto> AddCommentAsync(Guid ticketId, Guid userId, AddCommentDto dto)
        {
            var ticket = await _unitOfWork.Tickets.GetByIdAsync(ticketId);

            if (ticket == null)
                throw new Exception("Ticket not found");

            if (ticket.UserId != userId && ticket.AssignedEmployeeId != userId)
                throw new Exception("Not allowed");

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
            var comments = await _unitOfWork.TicketsComments
                .GetByTicketId(ticketId)
                .ToListAsync();

            return _mapper.Map<List<CommentDto>>(comments);
        }

        //RESOLVE
        public async Task ResolveTicketAsync(Guid ticketId, Guid employeeId)
        {
            var ticket = await _unitOfWork.Tickets.GetByIdAsync(ticketId);

            if (ticket == null)
                throw new Exception("Ticket not found");

            if (ticket.AssignedEmployeeId != employeeId)
                throw new Exception("Not assigned to you");

            ticket.Status = TicketStatus.Resolved;

            await _unitOfWork.SaveChangesAsync();
        }

        // CLOSE
        public async Task CloseTicketAsync(Guid ticketId, Guid clientId)
        {
            var ticket = await _unitOfWork.Tickets.GetByIdAsync(ticketId);

            if (ticket == null)
                throw new Exception("Ticket not found");

            if (ticket.UserId != clientId)
                throw new Exception("Not your ticket");

            if (ticket.Status != TicketStatus.Resolved)
                throw new Exception("Must be resolved first");

            ticket.Status = TicketStatus.Closed;

            await _unitOfWork.SaveChangesAsync();
        }

        //FILTER ALL 
        public async Task<List<TicketDto>> GetAllTicketsAsync(TicketFilterDto filter)
        {
            var query = _unitOfWork.Tickets.QueryTickets();

            if (!string.IsNullOrWhiteSpace(filter.Status) &&
                Enum.TryParse<TicketStatus>(filter.Status, out var status))
            {
                query = query.Where(t => t.Status == status);
            }

            if (filter.EmployeeId.HasValue)
                query = query.Where(t => t.AssignedEmployeeId == filter.EmployeeId);

            if (filter.ClientId.HasValue)
                query = query.Where(t => t.UserId == filter.ClientId);

            var tickets = await query.ToListAsync();

            return _mapper.Map<List<TicketDto>>(tickets);
        }

        //DETAILS
        public async Task<TicketDto?> GetTicketDetailsAsync(Guid ticketId)
        {
            var ticket = await _unitOfWork.Tickets.GetTicketWithDetailsAsync(ticketId);

            return ticket == null ? null : _mapper.Map<TicketDto>(ticket);
        }
    }
}