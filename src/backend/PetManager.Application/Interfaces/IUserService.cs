using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PetManager.Domain.Models;

namespace PetManager.Application.Interfaces;

public interface IUserService
{
    Task<User> CreateUserAsync(string name, string email, string cellphone, string document, int role, string username, string password);
    Task<User?> UpdateUserAsync(Guid id, string? name, string? email, string? cellphone, string? document, int? role, string? username, string? password, string? oldPassword);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User> GetUserAsync(Guid id);
    Task DeleteUserAsync(Guid id);
    Task InactivateUserAsync(Guid id);
    Task<User?> AuthenticateAsync(string username, string password);
}
