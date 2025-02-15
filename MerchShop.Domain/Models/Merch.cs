namespace MerchShop.Domain.Models;

public class Merch
{
    public int Id { get; set; }
    public string Type { get; set; } = String.Empty;
    public int Price { get; set; }

    public ICollection<Purchase> Purchases { get; set; } = [];
}