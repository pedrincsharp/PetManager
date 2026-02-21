using System;
using PetManager.Domain.Models.Enums;

namespace PetManager.Application.DTO;

public record UserResponseDto(
    Guid Id,
    string Name,
    string Email,
    string? Cellphone,
    string Document,
    Role Role,
    string Username,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    Status Status
);
