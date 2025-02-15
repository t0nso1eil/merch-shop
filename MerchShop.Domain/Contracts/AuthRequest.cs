namespace MerchShop.Domain.Contracts;

public record AuthRequest(
    string Username,
    string Password);