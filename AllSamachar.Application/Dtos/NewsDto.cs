namespace AllSamachar.Application.Dtos;

public class CategoryDto
{
    public Guid Id { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string AccentColor { get; set; } = string.Empty;
}

public class PublisherDto
{
    public Guid Id { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Website { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class NewsDto
{
    public Guid Id { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public CategoryDto Category { get; set; } = null!;
    public PublisherDto Publisher { get; set; } = null!;
    public string? Author { get; set; }
    public string OriginalUrl { get; set; } = string.Empty;
    public DateTime PublishedAt { get; set; }
    public string[] Tags { get; set; } = Array.Empty<string>();
    public string Status { get; set; } = string.Empty;
    public bool IsBreaking { get; set; }
    public int ViewCount { get; set; }
    public int ShareCount { get; set; }
    public int SaveCount { get; set; }
    public int OriginalClickCount { get; set; }
}