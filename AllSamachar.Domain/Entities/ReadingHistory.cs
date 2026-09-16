namespace AllSamachar.Domain.Entities;

public class ReadingHistory
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public Guid NewsId { get; set; }
    public News News { get; set; } = null!;

    public DateTime ViewedAt { get; set; } = DateTime.UtcNow;
}