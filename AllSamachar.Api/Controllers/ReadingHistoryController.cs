using System.Security.Claims;
using AllSamachar.Application.Dtos;
using AllSamachar.Application.Interfaces;
using AllSamachar.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AllSamachar.Api.Controllers;

[ApiController]
[Route("api/history")]
[Authorize]
public class ReadingHistoryController : ControllerBase
{
    private readonly IReadingHistoryRepository _historyRepository;

    public ReadingHistoryController(IReadingHistoryRepository historyRepository)
    {
        _historyRepository = historyRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetMyHistory([FromQuery] int limit = 20)
    {
        var userId = GetCurrentUserId();
        var history = await _historyRepository.GetByUserIdAsync(userId, limit);
        return Ok(history.Select(h => ToDto(h.News)));
    }

    [HttpPost("{newsId}")]
    public async Task<IActionResult> RecordVisit(Guid newsId)
    {
        var userId = GetCurrentUserId();

        await _historyRepository.AddAsync(new ReadingHistory
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            NewsId = newsId
        });

        return Ok();
    }

    private Guid GetCurrentUserId()
    {
        var sub = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        return Guid.Parse(sub!);
    }

    private static NewsDto ToDto(News n) => new()
    {
        Id = n.Id,
        Slug = n.Slug,
        Title = n.Title,
        Summary = n.Summary,
        ImageUrl = n.ImageUrl,
        Category = new CategoryDto { Id = n.Category.Id, Slug = n.Category.Slug, Name = n.Category.Name, AccentColor = n.Category.AccentColor },
        Publisher = new PublisherDto { Id = n.Publisher.Id, Slug = n.Publisher.Slug, Name = n.Publisher.Name, Website = n.Publisher.Website, Description = n.Publisher.Description },
        Author = n.Author,
        OriginalUrl = n.OriginalUrl,
        PublishedAt = n.PublishedAt,
        Tags = n.Tags,
        Status = n.Status.ToString(),
        IsBreaking = n.IsBreaking,
        ViewCount = n.ViewCount,
        ShareCount = n.ShareCount,
        SaveCount = n.SaveCount,
        OriginalClickCount = n.OriginalClickCount
    };
}