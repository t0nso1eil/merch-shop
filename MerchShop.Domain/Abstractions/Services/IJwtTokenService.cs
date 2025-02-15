using System.Security.Claims;
using MerchShop.Domain.Models;

namespace MerchShop.Domain.Abstractions.Services;

public interface IJwtTokenService
{
    string GenerateToken(User user);
    ClaimsPrincipal ValidateToken(string token);
}