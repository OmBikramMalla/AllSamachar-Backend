using AllSamachar.Domain.Entities;

namespace AllSamachar.Application.Interfaces;

public interface ISavedStoryRepository
{
    Task<List<SavedStory>> GetByUserIdAsync(Guid userId);
    Task<bool> IsSavedAsync(Guid userId, Guid newsId);

    Task AddAsync(SavedStory savedStory);
    Task RemoveAsync(Guid userId, Guid newsId);
}