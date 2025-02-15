using System.Security.Authentication;
using MerchShop.Domain.Abstractions.Services;
using MerchShop.Domain.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace MerchShop.API.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    public async Task<IActionResult> Authenticate([FromBody] AuthRequest request)
    {
        try
        {
            var response = await _authService.AuthenticateAsync(request);
            return Ok(response);
        }
        catch (InvalidCredentialException ex)
        {
            return Unauthorized(new ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponse(ex.Message));
        }
    }
}