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
            IsActive = true
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Guid id, UpdateCategoryRequestDto dto)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null || !category.IsActive)
            throw new Exception("Kategori bulunamadı.");

        category.Name = dto.Name;
        await _context.SaveChangesAsync();
    }

    public async Task SoftDeleteAsync(Guid id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category is null)
            throw new Exception("Kategori bulunamadı");

        category.IsActive = false;
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<CategoryDto>> GetActiveCategoriesAsync()
    {
        return await _context.Categories
            .Where(c => c.IsActive)
            .OrderBy(c => c.Name)
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name
            })
            .ToListAsync();
    }

}
