namespace AllSamachar.Domain.Entities;

public enum UserRole
{
    Reader,
    Contributor,
    Admin
}

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.Reader;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<SavedStory> SavedStories { get; set; } = new List<SavedStory>();
    public ICollection<ReadingHistory> ReadingHistory { get; set; } = new List<ReadingHistory>();
}
