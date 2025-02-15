using MerchShop.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MerchShop.Infrastructure.Configurations;

public class MerchConfiguration : IEntityTypeConfiguration<Merch>
{
    public void Configure(EntityTypeBuilder<Merch> builder)
    {
        builder.HasKey(m => m.Id);
        
        builder.Property(m => m.Type)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(m => m.Price)
            .IsRequired();

        builder.HasMany(m => m.Purchases)
            .WithOne(p => p.Merch)
            .HasForeignKey(p => p.MerchId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasData(
            new Merch { Id = 1, Type = "t-shirt", Price = 80 },
            new Merch { Id = 2, Type = "cup", Price = 20 },
            new Merch { Id = 3, Type = "book", Price = 50 },
            new Merch { Id = 4, Type = "pen", Price = 10 },
            new Merch { Id = 5, Type = "powerbank", Price = 200 },
            new Merch { Id = 6, Type = "hoody", Price = 300 },
            new Merch { Id = 7, Type = "umbrella", Price = 200 },
            new Merch { Id = 8, Type = "socks", Price = 10 },
            new Merch { Id = 9, Type = "wallet", Price = 50 },
            new Merch { Id = 10, Type = "pink-hoody", Price = 500 }
        );
    }
}