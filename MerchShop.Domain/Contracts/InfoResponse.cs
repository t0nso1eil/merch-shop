namespace MerchShop.Domain.Contracts;

public record InfoResponse(
    int Coins,
    List<InventoryItem> Inventory,
    CoinHistory CoinHistory
);