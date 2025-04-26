using PaparaGo.Application.Interfaces.Services;
using PaparaGo.Domain.Entities;
using PaparaGo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using PaparaGo.DTO;

namespace PaparaGo.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly PaparaGoDbContext _context;

    public CategoryService(PaparaGoDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _context.Categories.ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(Guid id)
    {
        return await _context.Categories.FindAsync(id);
    }

    public async Task CreateAsync(CreateCategoryRequestDto dto)
    {
        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            DeletedAt = null 
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Guid id, UpdateCategoryRequestDto dto)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null || category.DeletedAt != null)
            throw new Exception("Kategori bulunamadı veya pasif durumda.");

        category.Name = dto.Name;
        await _context.SaveChangesAsync();
    }

    public async Task SoftDeleteAsync(Guid id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null || category.DeletedAt != null)
            throw new Exception("Kategori bulunamadı veya zaten silinmiş.");

        // is there any expense pending
        bool hasActiveExpenses = await _context.ExpenseRequests
            .AnyAsync(e => e.CategoryId == id && e.Status == ExpenseStatus.Pending);

        if (hasActiveExpenses)
            throw new Exception("Bu kategoriye ait bekleyen masraf talepleri bulunduğu için silinemez.");

        category.DeletedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<CategoryDto>> GetActiveCategoriesAsync()
    {
        return await _context.Categories
            .Where(c => c.DeletedAt == null) // If DeletedAt null category is active
            .OrderBy(c => c.Name)
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name
            })
            .ToListAsync();
    }
}
