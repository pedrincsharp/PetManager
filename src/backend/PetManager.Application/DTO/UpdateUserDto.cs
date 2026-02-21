namespace PetManager.Application.DTO;

public record UpdateUserDto(string? Name, string? Email, string? Cellphone, string? Document, int? Role, string? Username, string? Password, string? OldPassword);
