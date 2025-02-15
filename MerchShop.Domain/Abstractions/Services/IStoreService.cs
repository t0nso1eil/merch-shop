namespace MerchShop.Domain.Abstractions.Services;

public interface IStoreService
{
    public Task BuyItemAsync(int employeeId, int itemId);
}