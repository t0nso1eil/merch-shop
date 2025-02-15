namespace MerchShop.Domain.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public int CoinsBalance { get; set; }

    public ICollection<Purchase> Purchases { get; set; } = [];

    public ICollection<Transaction> TransactionsSent { get; set; } = [];

    public ICollection<Transaction> TransactionsReceived { get; set; } = [];
}