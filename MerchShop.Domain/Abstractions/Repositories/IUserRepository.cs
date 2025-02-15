using MerchShop.Domain.Models;

namespace MerchShop.Domain.Abstractions.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByUsernameAsync (string username);
    Task<IEnumerable<User>> GetAllAsync();
    Task<int?> AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(int id);
}