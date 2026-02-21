using System.ComponentModel.DataAnnotations;

namespace PetManager.Application.DTO;

public record CreateUserDto(

    [param: Required]
    string Name,

    [param: Required]
    [param: EmailAddress]
    string Email,

    string? Cellphone,

    [param: Required]
    string Document,

    [param: Required]
    int Role,

    [param: Required]
    string Username,

    [param: Required]
    string Password
);