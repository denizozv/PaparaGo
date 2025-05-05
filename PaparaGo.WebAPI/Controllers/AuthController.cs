using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaparaGo.Application.DTO;
using PaparaGo.Application.Interfaces.Services;
using PaparaGo.DTO;

namespace PaparaGo.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var token = await _authService.LoginAsync(dto);
        return Ok(new { token });
    }

    [HttpPut("change-password")]
    [Authorize(Roles = "Personel")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
            return Unauthorized("Token geçersiz.");

        await _authService.ChangePasswordAsync(Guid.Parse(userId), dto.OldPassword, dto.NewPassword);
        return Ok("Şifre başarıyla değiştirildi.");
    }

}


