using AllSamachar.Application.Interfaces;
using AllSamachar.Domain.Entities;
using AllSamachar.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AllSamachar.Infrastructure.Repositories;

public class SavedStoryRepository : ISavedStoryRepository
{
    private readonly AppDbContext _context;

    public SavedStoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<SavedStory>> GetByUserIdAsync(Guid userId)
    {
        return await _context.SavedStories
            .Include(s => s.News)
                .ThenInclude(n => n.Category)
            .Include(s => s.News)
                .ThenInclude(n => n.Publisher)
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.SavedAt)
            .ToListAsync();
    }

    public async Task<bool> IsSavedAsync(Guid userId, Guid newsId)
    {
        return await _context.SavedStories
            .AnyAsync(s => s.UserId == userId && s.NewsId == newsId);
    }

    public async Task AddAsync(SavedStory savedStory)
    {
        _context.SavedStories.Add(savedStory);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(Guid userId, Guid newsId)
    {
        var entry = await _context.SavedStories
            .FirstOrDefaultAsync(s => s.UserId == userId && s.NewsId == newsId);

        if (entry != null)
        {
            _context.SavedStories.Remove(entry);
            await _context.SaveChangesAsync();
        }
    }
}