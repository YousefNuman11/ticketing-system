using AutoMapper;
using TicketingSystem.Repository.Models;
using TicketingSystem.Services.DTOs.AuthenticationDto;
using TicketingSystem.Services.DTOs.ProductDtos;
using TicketingSystem.Services.DTOs.TicketDtos;
using TicketingSystem.Services.DTOs.User;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>();

        CreateMap<RegisterDto, User>();
        CreateMap<CreateEmployeeDto, User>();
        CreateMap<UpdateUserDto, User>();


        CreateMap<CreateTicketDto, Ticket>();
        CreateMap<Ticket, TicketDto>()
        .ForMember(
            d => d.Status,
            o => o.MapFrom(s => s.Status.ToString())
        );

        CreateMap<CreateProductDto, Product>();
        CreateMap<Product, ProductDto>();
    }
}