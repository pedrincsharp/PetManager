using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PetManager.Application.Interfaces;
using PetManager.Application.DTO;
using PetManager.Domain.Models;

namespace PetManager.Application.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public TokenResponseDto GenerateTokens(ApiKey apiKey)
    {
        var accessToken = GenerateAccessToken(apiKey);
        var refreshToken = GenerateRefreshToken();
        var expirationMinutes = int.TryParse(_configuration.GetSection("Jwt")["AccessTokenExpirationMinutes"], out var m) ? m : 60;
        var expiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes);

        return new TokenResponseDto(accessToken, refreshToken, expiresAt);
    }

    public string GenerateAccessToken(ApiKey apiKey)
    {
        var jwtSection = _configuration.GetSection("Jwt");
        var key = jwtSection["Key"] ?? throw new InvalidOperationException("Jwt:Key is not configured");
        var issuer = jwtSection["Issuer"] ?? "PetManager";
        var audience = jwtSection["Audience"] ?? "PetManagerClients";
        var minutes = int.TryParse(jwtSection["AccessTokenExpirationMinutes"], out var m) ? m : 60;

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, apiKey.Id.ToString()),
            new Claim("api_key_description", apiKey.Description),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(minutes);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}

