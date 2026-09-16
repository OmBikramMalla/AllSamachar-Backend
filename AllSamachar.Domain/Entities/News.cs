namespace AllSamachar.Domain.Entities;

public enum NewsStatus
{
    Draft,
    Pending,
    Approved,
    Rejected
}

public class News
{
    public Guid Id { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;

    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public Guid PublisherId { get; set; }
    public Publisher Publisher { get; set; } = null!;

    public string? Author { get; set; }
    public string OriginalUrl { get; set; } = string.Empty;
    public DateTime PublishedAt { get; set; }
    public string[] Tags { get; set; } = Array.Empty<string>();

    public NewsStatus Status { get; set; } = NewsStatus.Draft;
    public bool IsBreaking { get; set; }

    public int ViewCount { get; set; }
    public int ShareCount { get; set; }
    public int SaveCount { get; set; }
    public int OriginalClickCount { get; set; }
}