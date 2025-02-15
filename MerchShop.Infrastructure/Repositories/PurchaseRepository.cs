using System.Data.SqlClient;
using MerchShop.Domain.Abstractions.Repositories;
using MerchShop.Domain.Models;

namespace MerchShop.Infrastructure.Repositories;

public class PurchaseRepository : IPurchaseRepository
{
    private readonly ShopDbContext _context;

    public PurchaseRepository(ShopDbContext context)
    {
        _context = context;
    }
    
    public async Task<Purchase?> GetByIdAsync(int id)
    {
        return await _context.Purchases.FindAsync(id);
    }

    public async Task<int?> AddAsync(Purchase purchase)
    {
        await _context.Purchases.AddAsync(purchase);
        await _context.SaveChangesAsync();
        return purchase.Id;
    }

    public async Task UpdateAsync(Purchase purchase)
    {
        _context.Purchases.Update(purchase);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var purchase = await _context.Purchases.FindAsync(id);
        if (purchase != null)
        {
            _context.Purchases.Remove(purchase);
            await _context.SaveChangesAsync();
        }
    }
}