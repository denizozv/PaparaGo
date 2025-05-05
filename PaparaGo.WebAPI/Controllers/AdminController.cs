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
        [HttpGet("reports/weekly-expenses")]
        public async Task<IActionResult> GetWeeklyExpenses()
        {
            var report = await _reportService.GetWeeklyExpenseSummaryAsync();
            return Ok(report);
        }



    }
}
