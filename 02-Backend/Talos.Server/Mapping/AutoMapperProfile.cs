using AutoMapper;
using Talos.Shared.Models;
using Talos.Server.Models.Dtos;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        //You define all the mapping rules between your entities (models) and DTOs.
        // Template mappings IMPORTANT :avoiding manually writing repetitive property assignment code.
        CreateMap<Template, TemplateDto>();
        CreateMap<TemplateCreateDto, Template>();
        
        // User mappings
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<UserCreateDto, User>();
        CreateMap<UserUpdateDto, User>();
    }
}