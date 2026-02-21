using System;
using PetManager.Domain.Models.Enums;

namespace PetManager.Domain.Models;

public class ModelBase
{
    public Guid Id { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public Status Status { get; private set; }

    public ModelBase()
    {
        Id = Guid.CreateVersion7();
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        Status = Status.Active;
    }

    public void ChangeStatus(Status status)
    {
        Status = status;
        this.UpdateTimestamps();
    }
    public void UpdateTimestamps() => UpdatedAt = DateTime.UtcNow;
}
