using PetManager.Application.DTO;
using PetManager.Domain.Models;

namespace PetManager.Application.Interfaces;

public interface ITokenService
{
    TokenResponseDto GenerateTokens(ApiKey apiKey);
    TokenResponseDto GenerateTokensForUser(User user);
    string GenerateAccessToken(ApiKey apiKey);
    string GenerateAccessTokenForUser(User user);
}
