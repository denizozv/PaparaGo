using PaparaGo.Domain.Entities;

namespace PaparaGo.Application.Interfaces.Services;

public interface ICategoryService
{
    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(Guid id);
    Task CreateAsync(Category category);
    Task UpdateAsync(Category category);
    Task SoftDeleteAsync(Guid id); // Soft delete 
}
