using MerchShop.Domain.Abstractions.Repositories;
using MerchShop.Domain.Abstractions.Services;
using MerchShop.Domain.Contracts;
using MerchShop.Domain.Models;

namespace MerchShop.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<string> AuthenticateAsync(AuthRequest request)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username);
        if (user == null)
        {
            user = new User
            {
                Username = request.Username,
                PasswordHash = request.Password,
                CoinsBalance = 0
            };
            await Register(user);
        }

        if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid password.");
        }

        var token = _jwtTokenService.GenerateToken(user);
        return token;
    }

    public async Task Register(User user)
    {
        if (user.Username == null || user.PasswordHash == null)
        {
            throw new InvalidOperationException("Invalid user data.");
        }
        user.PasswordHash = _passwordHasher.HashPassword(user.PasswordHash);
        await _userRepository.AddAsync(user);
    }
    
    public async Task<InfoResponse> GetInfo(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) throw new InvalidOperationException("User not found.");


        var itemsTypes = user.Purchases
            .GroupBy(p => p.Merch.Type);
        var items = itemsTypes.Select(g => new InventoryItem(g.Key,g.Count())).ToList();


        var receivedTransactions = user.TransactionsReceived
            .Select(ct => new ReceivedCoins(ct.FromUser.Username ?? "Admin", ct.CoinsAmount)).ToList();
        var sentTransactions = user.TransactionsSent
            .Select(ct => new SendCoins(ct.ToUser.Username ?? "Admin",ct.CoinsAmount)).ToList();

        var coinsHistory = new CoinHistory(receivedTransactions, sentTransactions);
        
        return new InfoResponse(user.CoinsBalance,items,coinsHistory);
    }

}