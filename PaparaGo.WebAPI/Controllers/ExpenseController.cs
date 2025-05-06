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
    private readonly IReportService _reportService;

    public ExpenseController(IExpenseService expenseService, IReportService reportService)
    {
        _expenseService = expenseService;
        _reportService = reportService;
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

    [HttpGet("expenses/pending")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetPendingExpenses()
    {
        var pendingExpenses = await _expenseService.GetPendingRequestsAsync();
        return Ok(pendingExpenses);
    }

    [HttpGet("expenses/approved")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetApprovedExpenses()
    {
        var approvedExpenses = await _expenseService.GetApprovedRequestsAsync();
        return Ok(approvedExpenses);
    }


    [HttpGet("expenses/rejected")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetRejectedExpenses()
    {
        var rejectedExpenses = await _expenseService.GetRejectedRequestsAsync();
        return Ok(rejectedExpenses);
    }

    [HttpGet("personel/daily")]
    [Authorize(Roles = "Personel")]
    public async Task<IActionResult> GetDailyExpenses()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
            return Unauthorized("Token geçersiz.");

        var all = await _reportService.GetPersonalDailyExpensesAsync(Guid.Parse(userId));
        var today = DateTime.Today;
        var filtered = all.Where(x => x.Date.Date == today).ToList();
        return Ok(filtered);
    }

    [HttpGet("personel/weekly")]
    [Authorize(Roles = "Personel")]
    public async Task<IActionResult> GetWeeklyExpenses()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
            return Unauthorized("Token geçersiz.");

        var all = await _reportService.GetPersonalWeeklyExpensesAsync(Guid.Parse(userId));
        
        var sevenDaysAgo = DateTime.Today.Subtract(TimeSpan.FromDays(7));

        var filtered = all.Where(x => x.Date >= sevenDaysAgo && x.Date < DateTime.Today).ToList();

        return Ok(filtered);
    }

    [HttpGet("personel/monthly")]
    [Authorize(Roles = "Personel")]
    public async Task<IActionResult> GetMonthlyExpenses()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
            return Unauthorized("Token geçersiz.");

        var all = await _reportService.GetPersonalMonthlyExpensesAsync(Guid.Parse(userId));
        var today = DateTime.Today;
        var firstOfMonth = new DateTime(today.Year, today.Month, 1);
        var firstOfNextMonth = firstOfMonth.AddMonths(1);

        var filtered = all.Where(x => x.Date >= firstOfMonth && x.Date < firstOfNextMonth).ToList();
        return Ok(filtered);
    }




}