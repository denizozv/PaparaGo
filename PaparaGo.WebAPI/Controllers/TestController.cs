using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PaparaGo.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet("auth-check")]
    [Authorize] 
    public IActionResult CheckAuth()
    {
        return Ok("Token geçerli, yetkilendirme başarılı!");
    }

    [HttpGet("admin-check")]
    [Authorize(Roles = "Admin")] 
    public IActionResult CheckAdmin()
    {
        return Ok("Admin yetkisiyle bu endpoint'e eriştin!");
    }

    [HttpGet("personel-check")]
    [Authorize(Roles = "Personel")]
    public IActionResult CheckPersonel()
    {
        return Ok("Personel rolüyle bu endpoint'e eriştin!");
    }
}
