using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaparaGo.Application.Interfaces.Services;
using PaparaGo.DTO;

namespace PaparaGo.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IExpenseService _expenseService;
        private readonly IAuthService _authService;

        private readonly IReportService _reportService;

        public AdminController(ICategoryService categoryService, IExpenseService expenseService, IAuthService authService, IReportService reportService)
        {
            _categoryService = categoryService;
            _expenseService = expenseService;
            _authService = authService;
            _reportService = reportService;
        }

        //soft delete 
        [HttpDelete("categories/{id}")]
        public async Task<IActionResult> SoftDeleteCategory(Guid id)
        {
            await _categoryService.SoftDeleteAsync(id);
            return NoContent();
        }

        [HttpPut("expense/approve")]
        public async Task<IActionResult> Approve([FromBody] ApproveExpenseRequestDto dto)
        {
            await _expenseService.ApproveAsync(dto.ExpenseRequestId);
            return Ok("Masraf talebi onaylandı ve ödeme kaydı oluşturuldu.");
        }

        [HttpPut("expense/reject")]
        public async Task<IActionResult> Reject([FromBody] RejectExpenseRequestDto dto)
        {
            await _expenseService.RejectAsync(dto.ExpenseRequestId, dto.RejectionReason);
            return Ok("Masraf talebi reddedildi.");
        }

        [HttpPost("categories")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequestDto dto)
        {
            await _categoryService.CreateAsync(dto);
            return Ok("Kategori başarıyla oluşturuldu.");
        }

        [HttpPut("categories/{id}")]
        public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] UpdateCategoryRequestDto dto)
        {
            await _categoryService.UpdateAsync(id, dto);
            return Ok("Kategori başarıyla güncellendi.");
        }

        [HttpPost("personels")]
        public async Task<IActionResult> CreatePersonel([FromBody] CreatePersonelRequestDto dto)
        {
            await _authService.RegisterPersonelAsync(dto);
            return Ok("Personel başarıyla oluşturuldu.");
        }

        
        [HttpGet("reports/company-daily")]
        public async Task<IActionResult> GetCompanyDailyExpenses()
        {
            var report = await _reportService.GetCompanyDailyExpensesAsync();
            return Ok(report);
        }

        
        [HttpGet("reports/company-weekly")]
        public async Task<IActionResult> GetCompanyWeeklyExpenses()
        {
            var report = await _reportService.GetCompanyWeeklyExpensesAsync();
            return Ok(report);
        }

        
        [HttpGet("reports/company-monthly")]
        public async Task<IActionResult> GetCompanyMonthlyExpenses()
        {
            var report = await _reportService.GetCompanyMonthlyExpensesAsync();
            return Ok(report);
        }


        [HttpGet("reports/category-distribution")]
        public async Task<IActionResult> GetCategoryDistribution() =>
            Ok(await _reportService.GetCategoryDistributionAsync());

        [HttpGet("reports/top-spenders")]
        public async Task<IActionResult> GetTopSpenders() =>
            Ok(await _reportService.GetTopSpendersAsync());

        [HttpGet("reports/daily-averages")]
        public async Task<IActionResult> GetDailyAverages() =>
            Ok(await _reportService.GetDailyAveragesAsync());




    }
}
