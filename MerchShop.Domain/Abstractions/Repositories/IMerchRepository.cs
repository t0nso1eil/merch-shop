using MerchShop.Domain.Models;

namespace MerchShop.Domain.Abstractions.Repositories;

public interface IMerchRepository
{
    Task<Merch?> GetByIdAsync(int id);
    Task<IEnumerable<Merch>> GetAllAsync();
    Task<int?> AddAsync(Merch merch);
    Task UpdateAsync(Merch merch);
    Task DeleteAsync(int id);
}