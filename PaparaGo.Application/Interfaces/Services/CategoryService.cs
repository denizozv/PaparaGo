using PaparaGo.Application.Interfaces.Services;
using PaparaGo.Domain.Entities;
using PaparaGo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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

    public async Task CreateAsync(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Category category)
    {
        _context.Categories.Update(category);
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
}
