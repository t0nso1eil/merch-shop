namespace MerchShop.Domain.Contracts;

public record SendCoins(
    string ToUser,
    int Amount);