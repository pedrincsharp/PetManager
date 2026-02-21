using AutoMapper;
using PetManager.Domain.Models;
using PetManager.Application.DTO;

namespace PetManager.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserResponseDto>().ReverseMap();
    }
}
