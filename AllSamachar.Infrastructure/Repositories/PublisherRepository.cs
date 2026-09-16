using AllSamachar.Application.Interfaces;
using AllSamachar.Domain.Entities;
using AllSamachar.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AllSamachar.Infrastructure.Repositories;

public class PublisherRepository : IPublisherRepository
{
    private readonly AppDbContext _context;

    public PublisherRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Publisher>> GetAllAsync()
    {
        return await _context.Publishers.ToListAsync();
    }

    public async Task<Publisher?> GetBySlugAsync(string slug)
    {
        return await _context.Publishers.FirstOrDefaultAsync(p => p.Slug == slug);
    }

    public async Task<int> GetNewsCountAsync(Guid publisherId)
    {
        return await _context.News.CountAsync(n => n.PublisherId == publisherId);
    }

    public async Task AddAsync(Publisher publisher)
    {
        _context.Publishers.Add(publisher);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Publisher publisher)
    {
        _context.Publishers.Update(publisher);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var publisher = await _context.Publishers.FindAsync(id);
        if (publisher != null)
        {
            _context.Publishers.Remove(publisher);
            await _context.SaveChangesAsync();
        }
    }
}