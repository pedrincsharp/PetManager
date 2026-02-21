namespace PetManager.Application.DTO;

public record TokenResponseDto(
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAt
);
