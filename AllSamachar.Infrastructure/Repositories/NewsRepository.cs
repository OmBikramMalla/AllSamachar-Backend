using AllSamachar.Application.Common;
using AllSamachar.Application.Interfaces;
using AllSamachar.Domain.Entities;
using AllSamachar.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AllSamachar.Infrastructure.Repositories;

public class NewsRepository : INewsRepository
{
    private readonly AppDbContext _context;

    public NewsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<News?> GetBySlugAsync(string slug)
    {
        return await _context.News
            .Include(n => n.Category)
            .Include(n => n.Publisher)
            .FirstOrDefaultAsync(n => n.Slug == slug);
    }

    public async Task<News?> GetByIdAsync(Guid id)
    {
        return await _context.News
            .Include(n => n.Category)
            .Include(n => n.Publisher)
            .FirstOrDefaultAsync(n => n.Id == id);
    }

    public async Task<PagedResult<News>> GetLatestAsync(int page, int pageSize)
    {
        var query = _context.News
            .Include(n => n.Category)
            .Include(n => n.Publisher)
            .Where(n => n.Status == NewsStatus.Approved)
            .OrderByDescending(n => n.PublishedAt);

        return await PaginateAsync(query, page, pageSize);
    }

    public async Task<PagedResult<News>> GetByCategoryAsync(string categorySlug, int page, int pageSize)
    {
        var query = _context.News
            .Include(n => n.Category)
            .Include(n => n.Publisher)
            .Where(n => n.Status == NewsStatus.Approved && n.Category.Slug == categorySlug)
            .OrderByDescending(n => n.PublishedAt);

        return await PaginateAsync(query, page, pageSize);
    }

    public async Task<PagedResult<News>> GetByPublisherAsync(string publisherSlug, int page, int pageSize)
    {
        var query = _context.News
            .Include(n => n.Category)
            .Include(n => n.Publisher)
            .Where(n => n.Status == NewsStatus.Approved && n.Publisher.Slug == publisherSlug)
            .OrderByDescending(n => n.PublishedAt);

        return await PaginateAsync(query, page, pageSize);
    }

    public async Task<PagedResult<News>> GetMostReadAsync(int page, int pageSize)
    {
        var query = _context.News
            .Include(n => n.Category)
            .Include(n => n.Publisher)
            .Where(n => n.Status == NewsStatus.Approved)
            .OrderByDescending(n => n.ViewCount);

        return await PaginateAsync(query, page, pageSize);
    }

    public async Task<News?> GetBreakingAsync()
    {
        return await _context.News
            .Include(n => n.Category)
            .Include(n => n.Publisher)
            .Where(n => n.Status == NewsStatus.Approved && n.IsBreaking)
            .FirstOrDefaultAsync();
    }

    public async Task<PagedResult<News>> SearchAsync(string query, string? categorySlug, int page, int pageSize)
    {
        var lowered = query.ToLower();

        var q = _context.News
            .Include(n => n.Category)
            .Include(n => n.Publisher)
            .Where(n => n.Status == NewsStatus.Approved)
            .Where(n => n.Title.ToLower().Contains(lowered)
                     || n.Summary.ToLower().Contains(lowered)
                     || n.Tags.Any(t => t.ToLower().Contains(lowered)));

        if (!string.IsNullOrEmpty(categorySlug))
        {
            q = q.Where(n => n.Category.Slug == categorySlug);
        }

        return await PaginateAsync(q.OrderByDescending(n => n.PublishedAt), page, pageSize);
    }

    public async Task AddAsync(News news)
    {
        _context.News.Add(news);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(News news)
    {
        _context.News.Update(news);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var news = await _context.News.FindAsync(id);
        if (news != null)
        {
            _context.News.Remove(news);
            await _context.SaveChangesAsync();
        }
    }

    private static async Task<PagedResult<News>> PaginateAsync(IQueryable<News> query, int page, int pageSize)
    {
        var totalCount = await query.CountAsync();
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PagedResult<News>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<PagedResult<News>> GetAllForAdminAsync(string? status, int page, int pageSize)
    {
        var query = _context.News
            .Include(n => n.Category)
            .Include(n => n.Publisher)
            .AsQueryable();

        if (!string.IsNullOrEmpty(status) && Enum.TryParse<NewsStatus>(status, true, out var parsedStatus))
        {
            query = query.Where(n => n.Status == parsedStatus);
        }

        return await PaginateAsync(query.OrderByDescending(n => n.PublishedAt), page, pageSize);
    }
}