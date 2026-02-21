using System;
using System.Threading.Tasks;
using PetManager.Application.DTO;
using PetManager.Application.Interfaces;
using PetManager.Domain.Models;
using PetManager.Infrastructure.Repositories;

namespace PetManager.Application.Services;

public class AuthService : IAuthService
{
    private readonly IApiKeyRepository _apiKeyRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly ITokenService _tokenService;

    public AuthService(
        IApiKeyRepository apiKeyRepository,
        IRefreshTokenRepository refreshTokenRepository,
        ITokenService tokenService)
    {
        _apiKeyRepository = apiKeyRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _tokenService = tokenService;
    }

    public async Task<TokenResponseDto> GenerateTokensByApiKeyAsync(string apiKey)
    {
        var apiKeyEntity = await _apiKeyRepository.GetByKeyAsync(apiKey);
        if (apiKeyEntity == null)
            throw new InvalidOperationException("Invalid API key");

        if (!apiKeyEntity.IsActive)
            throw new InvalidOperationException("API key is not active");

        // Generate tokens
        var tokenResponse = _tokenService.GenerateTokens(apiKeyEntity);

        // Create and save refresh token to database
        var refreshTokenEntity = new RefreshToken(apiKeyEntity.Id, tokenResponse.RefreshToken, 7);
        await _refreshTokenRepository.CreateAsync(refreshTokenEntity);

        return tokenResponse;
    }

    public async Task<TokenResponseDto> RefreshAccessTokenAsync(string refreshToken)
    {
        var refreshTokenEntity = await _refreshTokenRepository.GetByTokenAsync(refreshToken);
        if (refreshTokenEntity == null)
            throw new InvalidOperationException("Invalid refresh token");

        if (!refreshTokenEntity.IsValid())
            throw new InvalidOperationException("Refresh token has expired or been revoked");

        var apiKey = refreshTokenEntity.ApiKey;
        if (apiKey == null)
            throw new InvalidOperationException("Associated API key not found");

        if (!apiKey.IsActive)
            throw new InvalidOperationException("Associated API key is not active");

        // Revoke old refresh token
        refreshTokenEntity.Revoke();
        await _refreshTokenRepository.UpdateAsync(refreshTokenEntity);

        // Generate new tokens
        var newTokenResponse = _tokenService.GenerateTokens(apiKey);

        // Create and save new refresh token
        var newRefreshTokenEntity = new RefreshToken(apiKey.Id, newTokenResponse.RefreshToken, 7);
        await _refreshTokenRepository.CreateAsync(newRefreshTokenEntity);

        return newTokenResponse;
    }
}
