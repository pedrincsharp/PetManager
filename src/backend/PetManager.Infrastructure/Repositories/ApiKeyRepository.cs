using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PetManager.Domain.Models;

namespace PetManager.Infrastructure.Repositories;

public class ApiKeyRepository : IApiKeyRepository
{
    private readonly AppDbContext _db;

    public ApiKeyRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<ApiKey?> GetByKeyAsync(string key)
    {
        return await _db.ApiKeys.FirstOrDefaultAsync(a => a.Key == key);
    }

    public async Task<ApiKey?> GetByIdAsync(Guid id)
    {
        return await _db.ApiKeys.FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<ApiKey> CreateAsync(ApiKey apiKey)
    {
        _db.ApiKeys.Add(apiKey);
        await _db.SaveChangesAsync();
        return apiKey;
    }

    public async Task<ApiKey> UpdateAsync(ApiKey apiKey)
    {
        _db.ApiKeys.Update(apiKey);
        await _db.SaveChangesAsync();
        return apiKey;
    }

    public async Task DeleteAsync(Guid id)
    {
        var apiKey = await GetByIdAsync(id);
        if (apiKey != null)
        {
            _db.ApiKeys.Remove(apiKey);
            await _db.SaveChangesAsync();
        }
    }
}
