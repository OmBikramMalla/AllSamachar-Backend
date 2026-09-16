using AllSamachar.Domain.Entities;

namespace AllSamachar.Application.Interfaces;

public interface IPublisherRepository
{
    Task<List<Publisher>> GetAllAsync();
    Task<Publisher?> GetBySlugAsync(string slug);
    Task<int> GetNewsCountAsync(Guid publisherId);

    Task AddAsync(Publisher publisher);
    Task UpdateAsync(Publisher publisher);
    Task DeleteAsync(Guid id);
}