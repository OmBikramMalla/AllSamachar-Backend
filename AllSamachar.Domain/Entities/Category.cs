namespace AllSamachar.Domain.Entities;

public class Category
{
    public Guid Id { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string AccentColor { get; set; } = string.Empty;

    public ICollection<News> News { get; set; } = new List<News>();
}