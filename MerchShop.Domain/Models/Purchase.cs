namespace MerchShop.Domain.Models;

public class Purchase
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int MerchId { get; set; }
    public User User { get; set; }
    public Merch Merch { get; set; }
}