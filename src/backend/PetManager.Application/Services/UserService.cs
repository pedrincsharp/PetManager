using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BCrypt.Net;
using PetManager.Application.Interfaces;
using PetManager.Domain.Models;
using PetManager.Infrastructure.Repositories;

namespace PetManager.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repo;

    public UserService(IUserRepository repo)
    {
        _repo = repo;
    }

    public async Task<User> CreateUserAsync(string name, string email, string cellphone, string document, int role, string username, string password)
    {
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
        var user = new User(name, email, cellphone, document, (PetManager.Domain.Models.Enums.Role)role, username, passwordHash);
        return await _repo.CreateAsync(user);
    }

    public async Task<User?> UpdateUserAsync(Guid id, string? name, string? email, string? cellphone, string? document, int? role, string? username, string? password, string? oldPassword)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null) throw new PetManager.Application.Exceptions.UserNotFoundException($"User with id '{id}' not found");

        if (!string.IsNullOrEmpty(name)) existing.ChangeName(name);
        if (!string.IsNullOrEmpty(email)) existing.ChangeEmail(email);
        if (!string.IsNullOrEmpty(cellphone)) existing.ChangeCellphone(cellphone);
        if (!string.IsNullOrEmpty(document)) existing.ChangeDocument(document);
        if (role.HasValue) existing.ChangeRole((PetManager.Domain.Models.Enums.Role)role.Value);
        if (!string.IsNullOrEmpty(username)) existing.ChangeUsername(username);
        if (!string.IsNullOrEmpty(password))
        {
            if (string.IsNullOrEmpty(oldPassword) || !BCrypt.Net.BCrypt.Verify(oldPassword, existing.PasswordHash))
                throw new ArgumentException("Old password is required and must match to change password");

            existing.ChangePasswordHash(BCrypt.Net.BCrypt.HashPassword(password));
        }

        return await _repo.UpdateAsync(existing);
    }

    public Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return _repo.GetAllAsync();
    }

    public async Task<User> GetUserAsync(Guid id)
    {
        var user = await _repo.GetByIdAsync(id);
        if (user == null) throw new PetManager.Application.Exceptions.UserNotFoundException($"User with id '{id}' not found");
        return user;
    }

    public async Task DeleteUserAsync(Guid id)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null) throw new PetManager.Application.Exceptions.UserNotFoundException($"User with id '{id}' not found");
        await _repo.DeleteAsync(id);
    }

    public async Task InactivateUserAsync(Guid id)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null) throw new PetManager.Application.Exceptions.UserNotFoundException($"User with id '{id}' not found");
        await _repo.InactivateAsync(id);
    }

    public async Task<User?> AuthenticateAsync(string username, string password)
    {
        var user = await _repo.GetByUsernameAsync(username);
        if (user == null) return null;
        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash)) return null;
        return user;
    }
}
