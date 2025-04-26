using Microsoft.AspNetCore.Mvc;
using PaparaGo.Application.Interfaces.Services;

namespace PaparaGo.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActiveCategories()
    {
        var categories = await _categoryService.GetActiveCategoriesAsync();
        return Ok(categories);
    }
}
