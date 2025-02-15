namespace MerchShop.Domain.Contracts;

public record ReceivedCoins(
    string FromUser,
    int Amount);