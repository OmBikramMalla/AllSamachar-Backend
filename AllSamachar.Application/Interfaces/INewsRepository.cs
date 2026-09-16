using AllSamachar.Application.Common;
using AllSamachar.Domain.Entities;

namespace AllSamachar.Application.Interfaces;

public interface INewsRepository
{
    Task<News?> GetBySlugAsync(string slug);
    Task<PagedResult<News>> GetLatestAsync(int page, int pageSize);
    Task<PagedResult<News>> GetByCategoryAsync(string categorySlug, int page, int pageSize);
    Task<PagedResult<News>> GetByPublisherAsync(string publisherSlug, int page, int pageSize);
    Task<PagedResult<News>> GetMostReadAsync(int page, int pageSize);
    Task<News?> GetBreakingAsync();
    Task<PagedResult<News>> SearchAsync(string query, string? categorySlug, int page, int pageSize);

    Task AddAsync(News news);
    Task UpdateAsync(News news);
    Task DeleteAsync(Guid id);
}