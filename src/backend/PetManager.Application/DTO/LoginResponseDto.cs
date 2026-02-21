using System;

namespace PetManager.Application.DTO;

public record LoginResponseDto(
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAt
);
