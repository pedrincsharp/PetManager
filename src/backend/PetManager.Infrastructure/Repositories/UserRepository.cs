using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PetManager.Domain.Models;
using PetManager.Domain.Models.Enums;

namespace PetManager.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;

    public UserRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<User> CreateAsync(User user)
    {
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return user;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _db.Users.AsNoTracking().Where(u => u.Status != Status.Deleted).ToListAsync();
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _db.Users.FirstOrDefaultAsync(u => u.Id == id && u.Status != Status.Deleted);
    }

    public async Task<User?> UpdateAsync(User user)
    {
        var existing = await _db.Users.FirstOrDefaultAsync(u => u.Id == user.Id && u.Status != Status.Deleted);
        if (existing == null) return null;

        existing.ChangeName(user.Name);
        existing.ChangeEmail(user.Email);
        existing.ChangeCellphone(user.Cellphone);
        existing.ChangeDocument(user.Document);
        existing.ChangeRole(user.Role);
        existing.ChangeUsername(user.Username);
        existing.ChangePasswordHash(user.PasswordHash);

        await _db.SaveChangesAsync();
        return existing;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _db.Users.FirstOrDefaultAsync(u => u.Username == username && u.Status != Status.Deleted);
    }

    public async Task DeleteAsync(Guid id)
    {
        var existing = await _db.Users.FirstOrDefaultAsync(u => u.Id == id && u.Status != Status.Deleted);
        if (existing == null) return;
        existing.ChangeStatus(Status.Deleted);
        await _db.SaveChangesAsync();
    }

    public async Task InactivateAsync(Guid id)
    {
        var existing = await _db.Users.FirstOrDefaultAsync(u => u.Id == id && u.Status == Status.Active);
        if (existing == null) return;
        existing.ChangeStatus(Status.Inactive);
        await _db.SaveChangesAsync();
    }
}
