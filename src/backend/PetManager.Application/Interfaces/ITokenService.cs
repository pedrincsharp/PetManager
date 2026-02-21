using PetManager.Application.DTO;
using PetManager.Domain.Models;

namespace PetManager.Application.Interfaces;

public interface ITokenService
{
    TokenResponseDto GenerateTokens(ApiKey apiKey);
    string GenerateAccessToken(ApiKey apiKey);
}
