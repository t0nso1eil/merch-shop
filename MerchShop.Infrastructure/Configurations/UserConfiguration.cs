using MerchShop.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MerchShop.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.Username).IsUnique();
        
        builder.Property(e => e.Username)
            .HasMaxLength(100);

        builder.Property(e => e.PasswordHash)
            .IsRequired();

        builder.Property(e => e.CoinsBalance)
            .IsRequired();
        
        builder.HasMany(e => e.Purchases)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.TransactionsSent)
            .WithOne(t => t.FromUser)
            .HasForeignKey(t => t.FromEmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.TransactionsReceived)
            .WithOne(t => t.ToUser)
            .HasForeignKey(t => t.ToEmployeeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}