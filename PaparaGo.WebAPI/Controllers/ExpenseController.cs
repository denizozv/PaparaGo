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
    public async Task<IActionResult> GetDailyExpenses([FromQuery] DateTime date)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            return Unauthorized("Token geçersiz.");

        var result = await _reportService.GetPersonalDailyExpensesAsync(userId, date);
        return Ok(result);
    }

    [HttpGet("personel/weekly")]
    [Authorize(Roles = "Personel")]
    public async Task<IActionResult> GetWeeklyExpenses([FromQuery] DateTime date)
    {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier); // Kullanıcının ID'sini alıyoruz
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized("Token geçersiz.");

            var result = await _reportService.GetPersonalWeeklyExpensesAsync(userId, date);
            return Ok(result);
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
        var startOfMonth = new DateTime(today.Year, today.Month, 1);
        var endOfMonth = startOfMonth.AddMonths(1);


        var filtered = all
            .Where(x => x.Month >= startOfMonth && x.Month < endOfMonth)
            .ToList();

        return Ok(filtered);
    }




}