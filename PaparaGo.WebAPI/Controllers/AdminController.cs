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

        public AdminController(ICategoryService categoryService, IExpenseService expenseService)
        {
            _categoryService = categoryService;
            _expenseService = expenseService;
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequestDto dto)
        {
            await _categoryService.CreateAsync(dto);
            return Ok("Kategori başarıyla oluşturuldu.");
        }

        [HttpPut("categories/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] UpdateCategoryRequestDto dto)
        {
            await _categoryService.UpdateAsync(id, dto);
            return Ok("Kategori başarıyla güncellendi.");
        }
    }
}
