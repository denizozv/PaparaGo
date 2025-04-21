using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaparaGo.Application.Interfaces.Services;
using PaparaGo.DTO;
using System.Security.Claims;

namespace PaparaGo.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExpenseController : ControllerBase
{
    private readonly IExpenseService _expenseService;

    public ExpenseController(IExpenseService expenseService)
    {
        _expenseService = expenseService;
    }

    //make expense 
    [HttpPost]
    [Authorize(Roles = "Personel")]
    public async Task<IActionResult> Create([FromBody] CreateExpenseRequestDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
            return Unauthorized("Token geçersiz.");

        await _expenseService.CreateAsync(dto, Guid.Parse(userId));
        return Ok("Masraf talebi başarıyla oluşturuldu.");
    }

    [HttpGet("mine")]
    [Authorize(Roles = "Personel")]
    public async Task<IActionResult> GetMyExpenses()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
            return Unauthorized("Token geçersiz.");

        var expenses = await _expenseService.GetMyExpensesAsync(Guid.Parse(userId));
        return Ok(expenses);
    }

}