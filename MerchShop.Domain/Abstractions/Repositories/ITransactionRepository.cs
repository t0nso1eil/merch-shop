using MerchShop.Domain.Models;

namespace MerchShop.Domain.Abstractions.Repositories;

public interface ITransactionRepository
{
    Task<Transaction?> GetByIdAsync(int id);
    Task<IEnumerable<Transaction>> GetByEmployeeIdAsync(int employeeId);
    Task<int?> AddAsync(Transaction transaction);
    Task UpdateAsync(Transaction transaction);
    Task DeleteAsync(int id);
}