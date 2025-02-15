using MerchShop.Domain.Models;

namespace MerchShop.Domain.Abstractions.Repositories;

public interface IPurchaseRepository
{
    Task<Purchase?> GetByIdAsync(int id);
    Task<int?> AddAsync(Purchase purchase);
    Task UpdateAsync(Purchase purchase);
    Task DeleteAsync(int id);
}