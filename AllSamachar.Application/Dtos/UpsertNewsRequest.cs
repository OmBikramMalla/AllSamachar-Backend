namespace AllSamachar.Application.Dtos;

public class UpsertNewsRequest
{
    public string Title { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public Guid CategoryId { get; set; }
    public Guid PublisherId { get; set; }
    public string? Author { get; set; }
    public string OriginalUrl { get; set; } = string.Empty;
    public DateTime? PublishedAt { get; set; }
    public string[] Tags { get; set; } = Array.Empty<string>();
    public string Status { get; set; } = "Draft";
    public bool IsBreaking { get; set; }
}