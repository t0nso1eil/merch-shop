using MerchShop.Domain.Abstractions.Repositories;
using MerchShop.Domain.Abstractions.Services;
using MerchShop.Domain.Models;

namespace MerchShop.Application.Services;

public class StoreService : IStoreService
{
    private readonly IUserRepository _userRepository;
    private readonly IMerchRepository _merchRepository;
    private readonly IPurchaseRepository _purchaseRepository;

    public StoreService(
        IUserRepository userRepository, 
        IMerchRepository merchRepository, 
        IPurchaseRepository purchaseRepository)
    {
        _userRepository = userRepository;
        _merchRepository = merchRepository;
        _purchaseRepository = purchaseRepository;
    }

    public async Task BuyItemAsync(int employeeId, int itemId)
    {
        var employee = await _userRepository.GetByIdAsync(employeeId);
        var merch = await _merchRepository.GetByIdAsync(itemId);

        if (employee == null)
            throw new InvalidOperationException("User not found.");

        if (merch == null)
            throw new InvalidOperationException("Item not found.");

        if (employee.CoinsBalance < merch.Price)
            throw new InvalidOperationException("Not enough coins.");

        employee.CoinsBalance -= merch.Price;
        await _userRepository.UpdateAsync(employee);

        var purchase = new Purchase
        {
            UserId = employeeId,
            MerchId = itemId,
        };

        await _purchaseRepository.AddAsync(purchase);
    }
}
