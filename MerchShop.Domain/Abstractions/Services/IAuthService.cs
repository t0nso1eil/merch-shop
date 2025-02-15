using MerchShop.Domain.Contracts;
using MerchShop.Domain.Models;

namespace MerchShop.Domain.Abstractions.Services;

public interface IAuthService
{
    public Task<string> AuthenticateAsync(AuthRequest request);
    public Task<InfoResponse> GetInfo(int userId);
}