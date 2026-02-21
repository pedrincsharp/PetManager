using System;
using System.Threading.Tasks;
using PetManager.Application.DTO;
using PetManager.Domain.Models;

namespace PetManager.Application.Interfaces;

public interface IAuthService
{
    Task<TokenResponseDto> GenerateTokensByApiKeyAsync(string apiKey);
    Task<TokenResponseDto> RefreshAccessTokenAsync(string refreshToken);
    Task<TokenResponseDto> LoginAsync(string username, string password);
}
