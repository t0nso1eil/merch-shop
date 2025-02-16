using MerchShop.Application.Services;
using MerchShop.Domain.Abstractions.Repositories;
using MerchShop.Domain.Models;
using Moq;
using Xunit;

namespace Tests;

public class WalletServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();
    private readonly Mock<ITransactionRepository> _transactionRepositoryMock = new Mock<ITransactionRepository>();
    private readonly WalletService _walletService;

    public WalletServiceTests()
    {
        _walletService = new WalletService(_userRepositoryMock.Object, _transactionRepositoryMock.Object);
    }

    [Fact]
    public async Task SendCoinsAsync_ValidTransaction_UpdatesBalances()
    {
        // Arrange
        var sender = new User { Id = 1, CoinsBalance = 100 };
        var receiver = new User { Id = 2, CoinsBalance = 50 };
        int amount = 30;

        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(sender.Id)).ReturnsAsync(sender);
        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(receiver.Id)).ReturnsAsync(receiver);

        // Act
        await _walletService.SendCoinsAsync(sender.Id, receiver.Id, amount);

        // Assert
        Assert.Equal(70, sender.CoinsBalance);
        Assert.Equal(80, receiver.CoinsBalance);
        
        _userRepositoryMock.Verify(repo => repo.UpdateAsync(sender), Times.Once);
        _userRepositoryMock.Verify(repo => repo.UpdateAsync(receiver), Times.Once);
        _transactionRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Transaction>()), Times.Once);
    }

    [Fact]
    public async Task SendCoinsAsync_NotEnoughBalance_ThrowsException()
    {
        // Arrange
        var sender = new User { Id = 1, CoinsBalance = 20 };
        var receiver = new User { Id = 2, CoinsBalance = 50 };
        int amount = 30;

        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(sender.Id)).ReturnsAsync(sender);
        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(receiver.Id)).ReturnsAsync(receiver);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _walletService.SendCoinsAsync(sender.Id, receiver.Id, amount));
    }

    [Fact]
    public async Task SendCoinsAsync_SenderNotFound_ThrowsException()
    {
        // Arrange
        var sender = new User { Id = 1, CoinsBalance = 100 };
        var receiver = new User { Id = 2, CoinsBalance = 50 };
        int amount = 30;

        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(sender.Id)).ReturnsAsync((User)null);  // Sender not found
        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(receiver.Id)).ReturnsAsync(receiver);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _walletService.SendCoinsAsync(sender.Id, receiver.Id, amount));
    }

    [Fact]
    public async Task SendCoinsAsync_ReceiverNotFound_ThrowsException()
    {
        // Arrange
        var sender = new User { Id = 1, CoinsBalance = 100 };
        var receiver = new User { Id = 2, CoinsBalance = 50 };
        int amount = 30;

        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(sender.Id)).ReturnsAsync(sender);
        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(receiver.Id)).ReturnsAsync((User)null);  // Receiver not found

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _walletService.SendCoinsAsync(sender.Id, receiver.Id, amount));
    }

    [Fact]
    public async Task SendCoinsAsync_NegativeAmount_ThrowsException()
    {
        // Arrange
        var sender = new User { Id = 1, CoinsBalance = 100 };
        var receiver = new User { Id = 2, CoinsBalance = 50 };
        int amount = -30;  // Negative amount

        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(sender.Id)).ReturnsAsync(sender);
        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(receiver.Id)).ReturnsAsync(receiver);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _walletService.SendCoinsAsync(sender.Id, receiver.Id, amount));
    }

    [Fact]
    public async Task SendCoinsAsync_ZeroAmount_ThrowsException()
    {
        // Arrange
        var sender = new User { Id = 1, CoinsBalance = 100 };
        var receiver = new User { Id = 2, CoinsBalance = 50 };
        int amount = 0;  // Zero amount

        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(sender.Id)).ReturnsAsync(sender);
        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(receiver.Id)).ReturnsAsync(receiver);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _walletService.SendCoinsAsync(sender.Id, receiver.Id, amount));
    }
}