using AllSamachar.Domain.Entities;

namespace AllSamachar.Application.Interfaces;

public interface IReadingHistoryRepository
{
    Task<List<ReadingHistory>> GetByUserIdAsync(Guid userId, int limit);
    Task AddAsync(ReadingHistory entry);
}