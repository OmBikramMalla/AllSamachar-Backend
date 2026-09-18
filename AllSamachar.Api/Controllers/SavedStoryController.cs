using System.Security.Claims;
using AllSamachar.Application.Dtos;
using AllSamachar.Application.Interfaces;
using AllSamachar.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AllSamachar.Api.Controllers;

[ApiController]
[Route("api/saved")]
[Authorize]
public class SavedStoryController : ControllerBase
{
    private readonly ISavedStoryRepository _savedStoryRepository;
    private readonly INewsRepository _newsRepository;

    public SavedStoryController(ISavedStoryRepository savedStoryRepository, INewsRepository newsRepository)
    {
        _savedStoryRepository = savedStoryRepository;
        _newsRepository = newsRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetMySaved()
    {
        var userId = GetCurrentUserId();
        var saved = await _savedStoryRepository.GetByUserIdAsync(userId);
        return Ok(saved.Select(s => ToDto(s.News)));
    }

    [HttpPost("{newsId}")]
    public async Task<IActionResult> Save(Guid newsId)
    {
        var userId = GetCurrentUserId();

        if (await _savedStoryRepository.IsSavedAsync(userId, newsId))
        {
            return Ok(); // already saved, treat as success rather than an error
        }

        await _savedStoryRepository.AddAsync(new SavedStory
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            NewsId = newsId
        });

        return Ok();
    }

    [HttpDelete("{newsId}")]
    public async Task<IActionResult> Unsave(Guid newsId)
    {
        var userId = GetCurrentUserId();
        await _savedStoryRepository.RemoveAsync(userId, newsId);
        return NoContent();
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