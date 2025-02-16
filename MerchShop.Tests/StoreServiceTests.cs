using MerchShop.Application.Services;
using MerchShop.Domain.Abstractions.Repositories;
using MerchShop.Domain.Models;
using Moq;
using Xunit;

namespace Tests;

public class StoreServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();
    private readonly Mock<IMerchRepository> _merchRepositoryMock = new Mock<IMerchRepository>();
    private readonly Mock<IPurchaseRepository> _purchaseRepositoryMock = new Mock<IPurchaseRepository>();
    private readonly StoreService _storeService;

    public StoreServiceTests()
    {
        _storeService = new StoreService(_userRepositoryMock.Object, _merchRepositoryMock.Object, _purchaseRepositoryMock.Object);
    }

    [Fact]
    public async Task BuyItemAsync_ValidPurchase_UpdatesBalanceAndAddsPurchase()
    {
        // Arrange
        var user = new User { Id = 1, CoinsBalance = 100 };
        var merch = new Merch { Id = 1, Price = 50 };
        
        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(user.Id)).ReturnsAsync(user);
        _merchRepositoryMock.Setup(repo => repo.GetByIdAsync(merch.Id)).ReturnsAsync(merch);
        
        // Act
        await _storeService.BuyItemAsync(user.Id, merch.Id);
        
        // Assert
        Assert.Equal(50, user.CoinsBalance); // Balance should be reduced by the price of the item.
        _userRepositoryMock.Verify(repo => repo.UpdateAsync(user), Times.Once);
        _purchaseRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Purchase>()), Times.Once);
    }

    [Fact]
    public async Task BuyItemAsync_InsufficientBalance_ThrowsException()
    {
        // Arrange
        var user = new User { Id = 1, CoinsBalance = 40 };  // Balance is less than the item's price
        var merch = new Merch { Id = 1, Price = 50 };
        
        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(user.Id)).ReturnsAsync(user);
        _merchRepositoryMock.Setup(repo => repo.GetByIdAsync(merch.Id)).ReturnsAsync(merch);
        
        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _storeService.BuyItemAsync(user.Id, merch.Id));
    }

    [Fact]
    public async Task BuyItemAsync_UserNotFound_ThrowsException()
    {
        // Arrange
        var user = new User { Id = 1, CoinsBalance = 100 };
        var merch = new Merch { Id = 1, Price = 50 };

        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(user.Id)).ReturnsAsync((User)null);  // User is not found
        _merchRepositoryMock.Setup(repo => repo.GetByIdAsync(merch.Id)).ReturnsAsync(merch);
        
        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _storeService.BuyItemAsync(user.Id, merch.Id));
    }

    [Fact]
    public async Task BuyItemAsync_ItemNotFound_ThrowsException()
    {
        // Arrange
        var user = new User { Id = 1, CoinsBalance = 100 };
        var merch = new Merch { Id = 1, Price = 50 };
        
        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(user.Id)).ReturnsAsync(user);
        _merchRepositoryMock.Setup(repo => repo.GetByIdAsync(merch.Id)).ReturnsAsync((Merch)null);  // Item is not found
        
        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _storeService.BuyItemAsync(user.Id, merch.Id));
    }
}