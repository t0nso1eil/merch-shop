namespace MerchShop.Domain.Models;

public class Transaction
{
    public int Id { get; set; }
    public int FromEmployeeId { get; set; } 
    public int ToEmployeeId { get; set; } 
    public int CoinsAmount { get; set; } 
    public User FromUser { get; set; }
    public User ToUser { get; set; }
}