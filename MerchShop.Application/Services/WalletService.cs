using MerchShop.Domain.Abstractions.Repositories;
using MerchShop.Domain.Abstractions.Services;
using MerchShop.Domain.Models;

namespace MerchShop.Application.Services;

public class WalletService : IWalletService
{
    private readonly IUserRepository _userRepository;
    private readonly ITransactionRepository _transactionRepository;

    public WalletService(IUserRepository userRepository, ITransactionRepository transactionRepository)
    {
        _userRepository = userRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task SendCoinsAsync(int fromEmployeeId, int toEmployeeId, int amount)
    {
        var fromUser = await _userRepository.GetByIdAsync(fromEmployeeId);
        var toUser = await _userRepository.GetByIdAsync(toEmployeeId);

        if (fromUser == null || toUser == null)
            throw new Exception("User not found.");
        
        if (amount <= 0)
            throw new Exception("Amount must be greater than zero.");

        if (fromUser.CoinsBalance < amount)
            throw new Exception("Not enough coins.");

        fromUser.CoinsBalance -= amount;
        toUser.CoinsBalance += amount;

        await _userRepository.UpdateAsync(fromUser);
        await _userRepository.UpdateAsync(toUser);

        var transaction = new Transaction
        {
            FromEmployeeId = fromEmployeeId,
            ToEmployeeId = toEmployeeId,
            CoinsAmount = amount,
        };
        await _transactionRepository.AddAsync(transaction);
    }
}
