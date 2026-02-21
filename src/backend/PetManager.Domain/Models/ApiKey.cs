using System;

namespace PetManager.Domain.Models;

public class ApiKey
{
    public Guid Id { get; private set; }
    public string Key { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public bool IsActive { get; private set; }

    protected ApiKey() { }

    public ApiKey(string key, string description)
    {
        Id = Guid.NewGuid();
        Key = key;
        Description = description;
        CreatedAt = DateTime.UtcNow;
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Activate()
    {
        IsActive = true;
    }
}
