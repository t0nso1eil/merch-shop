namespace MerchShop.Domain.Abstractions.Services;

public interface IWalletService
{
    public Task SendCoinsAsync(int fromEmployeeId, int toEmployeeId, int amount);
}