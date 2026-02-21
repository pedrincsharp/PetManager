using System;
using System.Threading.Tasks;
using PetManager.Domain.Models;

namespace PetManager.Infrastructure.Repositories;

public interface IApiKeyRepository
{
    Task<ApiKey?> GetByKeyAsync(string key);
    Task<ApiKey?> GetByIdAsync(Guid id);
    Task<ApiKey> CreateAsync(ApiKey apiKey);
    Task<ApiKey> UpdateAsync(ApiKey apiKey);
    Task DeleteAsync(Guid id);
}
