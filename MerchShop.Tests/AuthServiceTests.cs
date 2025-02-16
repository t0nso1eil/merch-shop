namespace Tests;

using Xunit;
using Moq;
using System.Threading.Tasks;
using MerchShop.Application.Services;
using MerchShop.Domain.Abstractions.Repositories;
using MerchShop.Domain.Abstractions.Services;
using MerchShop.Domain.Models;
using MerchShop.Domain.Contracts;
using System;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();
    private readonly Mock<IPasswordHasher> _passwordHasherMock = new Mock<IPasswordHasher>();
    private readonly Mock<IJwtTokenService> _jwtTokenServiceMock = new Mock<IJwtTokenService>();
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _authService = new AuthService(_userRepositoryMock.Object, _passwordHasherMock.Object, _jwtTokenServiceMock.Object);
    }

    [Fact]
    public async Task AuthenticateAsync_ValidUser_ReturnsToken()
    {
        // Arrange
        var request = new AuthRequest("testUser", "password123");
        var user = new User { Username = "testUser", PasswordHash = "hashedPassword", CoinsBalance = 100 };
        
        _userRepositoryMock.Setup(repo => repo.GetByUsernameAsync(request.Username)).ReturnsAsync(user);
        _passwordHasherMock.Setup(hasher => hasher.VerifyPassword(request.Password, user.PasswordHash)).Returns(true);
        _jwtTokenServiceMock.Setup(tokenService => tokenService.GenerateToken(user)).Returns("validToken");
        
        // Act
        var token = await _authService.AuthenticateAsync(request);
        
        // Assert
        Assert.Equal("validToken", token);
    }

    [Fact]
    public async Task AuthenticateAsync_InvalidPassword_ThrowsUnauthorizedException()
    {
        // Arrange
        var request = new AuthRequest("testUser","wrongPassword");
        var user = new User { Username = "testUser", PasswordHash = "hashedPassword" };
        
        _userRepositoryMock.Setup(repo => repo.GetByUsernameAsync(request.Username)).ReturnsAsync(user);
        _passwordHasherMock.Setup(hasher => hasher.VerifyPassword(request.Password, user.PasswordHash)).Returns(false);
        
        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _authService.AuthenticateAsync(request));
    }
}