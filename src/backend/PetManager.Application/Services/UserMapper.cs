using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using PetManager.Application.DTO;
using PetManager.Application.Interfaces;
using PetManager.Domain.Models;

namespace PetManager.Application.Services;

public class UserMapper : IUserMapper
{
    private readonly IMapper _mapper;

    public UserMapper(IMapper mapper)
    {
        _mapper = mapper;
    }

    public UserResponseDto ToDto(User user)
    {
        return _mapper.Map<UserResponseDto>(user);
    }

    public IEnumerable<UserResponseDto> ToDto(IEnumerable<User> users)
    {
        return users.Select(u => _mapper.Map<UserResponseDto>(u));
    }
}
