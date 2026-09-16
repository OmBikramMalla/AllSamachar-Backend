using AllSamachar.Domain.Entities;

namespace AllSamachar.Application.Interfaces;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync();
    Task<Category?> GetBySlugAsync(string slug);
    Task<int> GetNewsCountAsync(Guid categoryId);

    Task AddAsync(Category category);
    Task UpdateAsync(Category category);
    Task DeleteAsync(Guid id);
}