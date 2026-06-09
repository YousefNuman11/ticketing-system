using AutoMapper;
using TicketingSystem.Repository.Models;
using TicketingSystem.Services.DTOs;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>();

        CreateMap<RegisterDto, User>();
        CreateMap<CreateEmployeeDto, User>();

        CreateMap<UpdateUserDto, User>();
    }
}