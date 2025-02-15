using MerchShop.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MerchShop.Infrastructure.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(t => t.Id);
        
        builder.Property(t => t.CoinsAmount)
            .IsRequired();
        
        builder.HasOne(t => t.FromUser)
            .WithMany(e => e.TransactionsSent)
            .HasForeignKey(t => t.FromEmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.ToUser)
            .WithMany(e => e.TransactionsReceived)
            .HasForeignKey(t => t.ToEmployeeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}