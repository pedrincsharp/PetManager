using System;

namespace PetManager.Domain.Models;

public class RefreshToken
{
    public Guid Id { get; private set; }
    public Guid? ApiKeyId { get; private set; }
    public Guid? UserId { get; private set; }
    public string Token { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public bool IsRevoked { get; private set; }

    // Navigation properties
    public ApiKey? ApiKey { get; set; }
    public User? User { get; set; }

    protected RefreshToken() { }

    public RefreshToken(Guid apiKeyId, string token, int expirationDays = 7)
    {
        Id = Guid.NewGuid();
        ApiKeyId = apiKeyId;
        UserId = null;
        Token = token;
        CreatedAt = DateTime.UtcNow;
        ExpiresAt = DateTime.UtcNow.AddDays(expirationDays);
        IsRevoked = false;
    }

    public RefreshToken(Guid userId, string token, int expirationDays = 7, bool isForUser = false)
    {
        Id = Guid.NewGuid();
        ApiKeyId = isForUser ? null : userId;
        UserId = isForUser ? userId : null;
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
