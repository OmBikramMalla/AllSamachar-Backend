namespace AllSamachar.Domain.Entities;

public class Publisher
{
    public Guid Id { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Website { get; set; } = string.Empty;
    public string? Description { get; set; }

    public ICollection<News> News { get; set; } = new List<News>();
}