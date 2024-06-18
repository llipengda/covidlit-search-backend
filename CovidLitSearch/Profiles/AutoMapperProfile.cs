using AutoMapper;
using CovidLitSearch.Models;
using CovidLitSearch.Models.DTO;

namespace CovidLitSearch.Profiles;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<User, LoginDto>();
    }
}