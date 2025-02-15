namespace MerchShop.Domain.Contracts;

public record SendCoinRequest(
    string ToUser,
    int Amount);