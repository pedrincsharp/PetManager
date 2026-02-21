using System;

namespace PetManager.Domain.Models;

public class RefreshToken
{
    public Guid Id { get; private set; }
    public Guid ApiKeyId { get; private set; }
    public string Token { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public bool IsRevoked { get; private set; }

    // Navigation property
    public ApiKey? ApiKey { get; set; }

    protected RefreshToken() { }

    public RefreshToken(Guid apiKeyId, string token, int expirationDays = 7)
    {
        Id = Guid.NewGuid();
        ApiKeyId = apiKeyId;
        Token = token;
        CreatedAt = DateTime.UtcNow;
        ExpiresAt = DateTime.UtcNow.AddDays(expirationDays);
        IsRevoked = false;
    }

    public bool IsValid()
    {
        return !IsRevoked && ExpiresAt > DateTime.UtcNow;
    }

    public void Revoke()
    {
        IsRevoked = true;
    }
}
