using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PetManager.Domain.Models;

namespace PetManager.Infrastructure.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly AppDbContext _db;

    public RefreshTokenRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        return await _db.RefreshTokens
            .Include(r => r.ApiKey)
            .FirstOrDefaultAsync(r => r.Token == token);
    }

    public async Task<RefreshToken?> GetByIdAsync(Guid id)
    {
        return await _db.RefreshTokens
            .Include(r => r.ApiKey)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<RefreshToken> CreateAsync(RefreshToken refreshToken)
    {
        _db.RefreshTokens.Add(refreshToken);
        await _db.SaveChangesAsync();
        return refreshToken;
    }

    public async Task<RefreshToken> UpdateAsync(RefreshToken refreshToken)
    {
        _db.RefreshTokens.Update(refreshToken);
        await _db.SaveChangesAsync();
        return refreshToken;
    }

    public async Task DeleteAsync(Guid id)
    {
        var refreshToken = await GetByIdAsync(id);
        if (refreshToken != null)
        {
            _db.RefreshTokens.Remove(refreshToken);
            await _db.SaveChangesAsync();
        }
    }
}
