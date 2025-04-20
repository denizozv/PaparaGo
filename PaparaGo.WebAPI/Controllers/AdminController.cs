using Microsoft.AspNetCore.Mvc;
using PaparaGo.Application.Interfaces.Services;

namespace PaparaGo.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public AdminController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpDelete("categories/{id}")]
        public async Task<IActionResult> SoftDeleteCategory(Guid id)
        {
            await _categoryService.SoftDeleteAsync(id);
            return NoContent();
        }
    }
}
