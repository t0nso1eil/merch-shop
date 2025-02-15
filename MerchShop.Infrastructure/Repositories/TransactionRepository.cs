using System.Data.Entity;
using MerchShop.Domain.Abstractions.Repositories;
using MerchShop.Domain.Models;

namespace MerchShop.Infrastructure.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly ShopDbContext _context;

    public TransactionRepository(ShopDbContext context)
    {
        _context = context;
    }
    
    public async Task<Transaction?> GetByIdAsync(int id)
    {
        return await _context.Transactions.FindAsync(id);
    }

    public async Task<IEnumerable<Transaction>> GetByEmployeeIdAsync(int employeeId)
    {
        return await _context.Transactions
            .Where(t => t.FromEmployeeId == employeeId || t.ToEmployeeId == employeeId)
            .ToListAsync();
    }

    public async Task<int?> AddAsync(Transaction transaction)
    {
        await _context.Transactions.AddAsync(transaction);
        await _context.SaveChangesAsync();
        return transaction.Id;
    }

    public async Task UpdateAsync(Transaction transaction)
    {
        _context.Transactions.Update(transaction);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var transaction = await _context.Transactions.FindAsync(id);
        if (transaction != null)
        {
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
        }
    }
}