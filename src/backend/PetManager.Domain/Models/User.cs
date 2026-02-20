using System;
using PetManager.Domain.Models.Enums;

namespace PetManager.Domain.Models;

public class User : Person
{
    public string Username { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    protected User() : base() { }
    public User(string name, string email, string cellphone, string document, Role role, string username, string passwordHash)
        : base(name, email, cellphone, document, role)
    {
        Username = username;
        PasswordHash = passwordHash;
    }

    public void ChangeUsername(string username) => Username = username;
    public void ChangePasswordHash(string passwordHash) => PasswordHash = passwordHash;
}
