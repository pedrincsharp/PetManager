using System.ComponentModel.DataAnnotations;

namespace PetManager.Application.DTO;

public record LoginRequestDto(
    [param: Required]
    string Username,

    [param: Required]
    string Password
);
