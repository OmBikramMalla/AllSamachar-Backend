using AllSamachar.Application.Interfaces;
using AllSamachar.Domain.Entities;
using AllSamachar.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AllSamachar.Infrastructure.Repositories;

public class ReadingHistoryRepository : IReadingHistoryRepository
{
    private readonly AppDbContext _context;

    public ReadingHistoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ReadingHistory>> GetByUserIdAsync(Guid userId, int limit)
    {
        return await _context.ReadingHistory
            .Include(r => r.News)
                .ThenInclude(n => n.Category)
            .Include(r => r.News)
                .ThenInclude(n => n.Publisher)
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.ViewedAt)
            .Take(limit)
            .ToListAsync();
    }

    public async Task AddAsync(ReadingHistory entry)
    {
        _context.ReadingHistory.Add(entry);
        await _context.SaveChangesAsync();
    }
}