using MerchShop.Domain.Abstractions.Repositories;
using MerchShop.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MerchShop.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ShopDbContext _context;

    public UserRepository(ShopDbContext context)
    {
        _context = context;
    }
    
    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users
            .Include(u => u.Purchases)
            .ThenInclude(p => p.Merch)
            .Include(u => u.TransactionsReceived)
            .ThenInclude(t => t.FromUser)
            .Include(u => u.TransactionsSent)
            .ThenInclude(t => t.ToUser)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users
            .FirstOrDefaultAsync(e => e.Username == username);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }
    
    public async Task<int?> AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user.Id;
    }
    
    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(int id)
    {
        var employee = await _context.Users.FindAsync(id);
        if (employee != null)
        {
            _context.Users.Remove(employee);
            await _context.SaveChangesAsync();
        }
    }
}