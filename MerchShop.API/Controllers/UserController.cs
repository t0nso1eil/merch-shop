using System.Security.Claims;
using MerchShop.Domain.Abstractions.Services;
using MerchShop.Domain.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MerchShop.API.Controllers;

[Route("api")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IWalletService _walletService;
    private readonly IStoreService _storeService;

    public UserController(IAuthService authService, IWalletService walletService, IStoreService storeService)
    {
        _authService = authService;
        _walletService = walletService;
        _storeService = storeService;
    }

    [HttpGet("info")]
    [Authorize]
    public async Task<IActionResult> GetUserInfo()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized(new ErrorResponse("Unauthorized."));

        var info = await _authService.GetInfo(int.Parse(userId));
        return Ok(info);
    }

    [HttpPost("sendCoin")]
    [Authorize]
    public async Task<IActionResult> SendCoin([FromBody] SendCoinRequest request)
    {
        var senderId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (senderId == null)
            return Unauthorized(new ErrorResponse("Unauthorized."));

        await _walletService.SendCoinsAsync(int.Parse(senderId), int.Parse(request.ToUser), request.Amount);
        return Ok();
    }
    
    [HttpGet("buy/{item}")]
    [Authorize]
    public async Task<IActionResult> BuyItemAsync(string item)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized(new ErrorResponse("Unauthorized."));

        if (string.IsNullOrEmpty(item))
            return BadRequest(new ErrorResponse("Bad request."));

        try
        {
            await _storeService.BuyItemAsync(int.Parse(userId), int.Parse(item));
            return Ok();
        }
        catch (InvalidOperationException ex) 
        {
            return BadRequest(new ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponse(ex.Message));
        }
    }
}