using PaparaGo.Domain.Entities;
using PaparaGo.DTO;

namespace PaparaGo.Application.Interfaces.Services;

public interface ICategoryService
{
    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(Guid id);
    Task CreateAsync(CreateCategoryRequestDto dto);
    Task UpdateAsync(Guid id, UpdateCategoryRequestDto dto);

    Task SoftDeleteAsync(Guid id); // Soft delete 
    Task<IEnumerable<CategoryDto>> GetActiveCategoriesAsync();

}
