using MerchShop.Domain.Models;
using MerchShop.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace MerchShop.Infrastructure;

public class ShopDbContext : DbContext
{
    public ShopDbContext(DbContextOptions<ShopDbContext> options)
        : base(options) { }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Merch> Merch { get; set; }
    public DbSet<Purchase> Purchases { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new MerchConfiguration());
        modelBuilder.ApplyConfiguration(new PurchaseConfiguration());
        modelBuilder.ApplyConfiguration(new TransactionConfiguration());
    }
}