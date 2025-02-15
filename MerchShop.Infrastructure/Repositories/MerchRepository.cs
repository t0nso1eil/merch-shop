using System.Data.Entity;
using MerchShop.Domain.Abstractions.Repositories;
using MerchShop.Domain.Models;

namespace MerchShop.Infrastructure.Repositories;

public class MerchRepository : IMerchRepository
{
    private readonly ShopDbContext _context;

    public MerchRepository(ShopDbContext context)
    {
        _context = context;
    }
    
    public async Task<Merch?> GetByIdAsync(int id)
    {
        return await _context.Merch.FindAsync(id);
    }

    public async Task<IEnumerable<Merch>> GetAllAsync()
    {
        return await _context.Merch.ToListAsync();
    }

    public async Task<int?> AddAsync(Merch merch)
    {
        await _context.Merch.AddAsync(merch);
        await _context.SaveChangesAsync();
        return merch.Id;
    }

    public async Task UpdateAsync(Merch merch)
    {
        _context.Merch.Update(merch);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var merch = await _context.Merch.FindAsync(id);
        if (merch != null)
        {
            _context.Merch.Remove(merch);
            await _context.SaveChangesAsync();
        }
    }
}