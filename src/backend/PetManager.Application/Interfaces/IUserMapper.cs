using System.Collections.Generic;
using PetManager.Application.DTO;
using PetManager.Domain.Models;

namespace PetManager.Application.Interfaces;

public interface IUserMapper
{
    UserResponseDto ToDto(User user);
    IEnumerable<UserResponseDto> ToDto(IEnumerable<User> users);
}
