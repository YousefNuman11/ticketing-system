using AutoMapper;
using TicketingSystem.Repository.Models;
using TicketingSystem.Services.Features.AuthMediator.Contracts;
using TicketingSystem.Services.Features.ManagerMediator.Contracts;
using TicketingSystem.Services.Features.ProductMediator.Contracts;
using TicketingSystem.Services.Features.TicketMediator.Contracts;

namespace TicketingSystem.Services.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Users
            CreateMap<User, UserDto>();
            CreateMap<RegisterDto, User>();
            CreateMap<CreateEmployeeDto, User>();
            CreateMap<UpdateUserDto, User>();

            // Clients with their tickets (manager view)
            CreateMap<User, ClientWithTicketsDto>()
                .ForMember(d => d.Tickets, o => o.MapFrom(s => s.CreatedTickets));
            CreateMap<Ticket, ClientTicketDto>()
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));

            // Tickets
            CreateMap<CreateTicketDto, Ticket>();
            CreateMap<UpdateTicketDto, Ticket>();
            CreateMap<Ticket, TicketDto>()
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));

            // Products
            CreateMap<CreateProductDto, Product>();
            CreateMap<Product, ProductDto>();

            // Comments
            CreateMap<AddCommentDto, TicketsComment>();
            CreateMap<TicketsComment, CommentDto>();

            // Attachments
            CreateMap<TicketAttachment, AttachmentDto>();
        }
    }
}
